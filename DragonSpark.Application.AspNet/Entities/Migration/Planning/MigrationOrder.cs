using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public class MigrationOrder : ISelect<IModel, MigrationOrderResult>
{
	readonly ITopologicalSort _sort;

	protected MigrationOrder() : this(TopologicalSort.Default) {}

	public MigrationOrder(ITopologicalSort sort) => _sort = sort;

	public MigrationOrderResult Get(IModel parameter)
	{
		using var entities = parameter.GetEntityTypes()
		                              .Where(t => !t.IsOwned() && t.FindPrimaryKey() != null)
		                              .AsValueEnumerable()
		                              .ToArray(ArrayPool<IEntityType>.Shared);
		var result = _sort.Get(entities);
		return result;
	}
}

// TODO

public interface ITopologicalSort : ISelect<Lease<IEntityType>, MigrationOrderResult>;

public interface ITarjan : ISelect<Dictionary<IEntityType, HashSet<IEntityType>>, Cycles>;

sealed class Tarjan : ITarjan
{
	public static Tarjan Default { get; } = new();

	Tarjan() : this(EqualityComparer<IEntityType>.Default) {}

	readonly IEqualityComparer<IEntityType> _comparer;

	public Tarjan(IEqualityComparer<IEntityType> comparer) => _comparer = comparer;

	public Cycles Get(Dictionary<IEntityType, HashSet<IEntityType>> parameter)
	{
		var index   = 0;
		var stack   = new Stack<IEntityType>();
		var indices = new Dictionary<IEntityType, int>();
		var lowlink = new Dictionary<IEntityType, int>();
		var onStack = new HashSet<IEntityType>();
		var result  = new Cycles();

		foreach (var v in parameter.Keys)
		{
			if (!indices.ContainsKey(v))
			{
				StrongConnect(v);
			}
		}

		return result;

		void StrongConnect(IEntityType v)
		{
			indices[v] = index;
			lowlink[v] = index;
			index++;
			stack.Push(v);
			onStack.Add(v);

			foreach (var w in parameter[v])
			{
				if (!indices.TryGetValue(w, out var i))
				{
					StrongConnect(w);
					lowlink[v] = Math.Min(lowlink[v], lowlink[w]);
				}
				else if (onStack.Contains(w))
				{
					lowlink[v] = Math.Min(lowlink[v], i);
				}
			}

			if (lowlink[v] == indices[v])
			{
				var         scc = new List<IEntityType>();
				IEntityType w;
				do
				{
					w = stack.Pop();
					onStack.Remove(w);
					scc.Add(w);
				} while (!_comparer.Equals(w, v));

				result.Add(scc);
			}
		}
	}
}

public sealed class Cycles : List<List<IEntityType>>;

sealed class Dependencies : Select<IEntityType, HashSet<IEntityType>>
{
	public static Dependencies Default { get; } = new();

	Dependencies()
		: base(e => e.GetForeignKeys()
		             .Where(x => !x.IsOwnership)
		             .Select(x => x.PrincipalEntityType)
		             .Where(t => t.FindPrimaryKey() != null)
		             .ToHashSet()) {}
}

/*
sealed class TopologicalSort : ITopologicalSort
{
	public static TopologicalSort Default { get; } = new();

	TopologicalSort() : this(ComposeGraph.Default, ComposeDependents.Default, ComposeCycles.Default) {}

	readonly IComposeGraph               _graph;
	readonly IDetermineDependents        _dependents;
	readonly ISelect<Dependents, Cycles> _cycles;

	public TopologicalSort(IComposeGraph graph, IDetermineDependents dependents, ISelect<Dependents, Cycles> cycles)
	{
		_graph      = graph;
		_dependents = dependents;
		_cycles     = cycles;
	}

	public MigrationOrderResult Get(Lease<IEntityType> parameter)
	{
		var graph      = _graph.Get(parameter);
		var dependents = _dependents.Get(graph);
		var groups     = _cycles.Get(dependents);
		var linear     = groups.Where(x => x.Count == 1).SelectMany(x => x).Result();
		var cycles     = groups.Where(x => x.Count > 1).Select(x => x.Result()).Result();
		return new(linear, cycles, graph);
	}
}
*/
sealed class TopologicalSort : ITopologicalSort
{
	public static TopologicalSort Default { get; } = new();

	TopologicalSort() : this(ComposeGraph.Default, ComposeDependents.Default, ComposeCycles.Default) {}

	readonly IComposeGraph               _graph;
	readonly IDetermineDependents        _dependents;
	readonly ISelect<Dependents, Cycles> _cycles;

	public TopologicalSort(IComposeGraph graph, IDetermineDependents dependents, ISelect<Dependents, Cycles> cycles)
	{
		_graph      = graph;
		_dependents = dependents;
		_cycles     = cycles;
	}

	public MigrationOrderResult Get(Lease<IEntityType> parameter)
	{
		var graph      = _graph.Get(parameter);
		var dependents = _dependents.Get(graph);
		var groups     = _cycles.Get(dependents);

		// Acyclic entities: SCCs of size 1
		var linear = groups
		             .Where(x => x.Count == 1)
		             .SelectMany(x => x)
		             .Result();

		// Cycles: SCCs of size > 1
		var cycles = groups
		             .Where(x => x.Count > 1)
		             .Select(x => x.Result())
		             .Result();

		return new(linear, cycles, graph);
	}
}

public interface IComposeGraph : ISelect<Lease<IEntityType>, Dictionary<IEntityType, HashSet<IEntityType>>>;

sealed class ComposeGraph : IComposeGraph
{
	public static ComposeGraph Default { get; } = new();

	ComposeGraph() : this(Dependencies.Default.Get) {}

	readonly Func<IEntityType, HashSet<IEntityType>> _dependencies;

	public ComposeGraph(Func<IEntityType, HashSet<IEntityType>> dependencies) => _dependencies = dependencies;

	public Dictionary<IEntityType, HashSet<IEntityType>> Get(Lease<IEntityType> parameter)
	{
		var result = parameter.ToDictionary(x => x, _dependencies);
		
		foreach (var key in result.Keys)
		{
			result[key].RemoveWhere(x => !result.Keys.Contains(x));
		}

		return result;
	}
}

/// <summary>
/// ATTRIBUTION: Copilot
/// </summary>
// -------------------------------
// Tarjan SCC implementation
// -------------------------------

// -------------------------------
// Topological sort of SCC groups
// -------------------------------
sealed class ComposeCycles : ISelect<Dependents, Cycles>
{
	public static ComposeCycles Default { get; } = new();

	ComposeCycles() {}

	public Cycles Get(Dependents parameter)
	{
		var result    = new Cycles();
		var remaining = parameter.ToDictionary(x => x.Key, x => x.Value.ToHashSet());

		while (true)
		{
			using var ready = remaining.AsValueEnumerable()
			                           .Where(x => x.Value.Count == 0) // no dependencies
			                           .Select(x => x.Key)
			                           .ToArray(ArrayPool<List<IEntityType>>.Shared);

			if (ready.Length == 0)
			{
				break;
			}

			foreach (var r in ready)
			{
				result.Add(r);
				remaining.Remove(r);

				foreach (var d in remaining.Values)
				{
					d.Remove(r);
				}
			}
		}

		result.AddRange(remaining.Keys); // remaining are cyclic groups

		return result;
	}
}

public sealed class Dependents : Dictionary<List<IEntityType>, HashSet<List<IEntityType>>>;

public interface IDetermineDependents : ISelect<Dictionary<IEntityType, HashSet<IEntityType>>, Dependents>;

sealed class ComposeDependents : IDetermineDependents
{
	public static ComposeDependents Default { get; } = new();

	ComposeDependents() : this(Tarjan.Default) {}

	readonly ITarjan _tarjan;

	public ComposeDependents(ITarjan tarjan) => _tarjan = tarjan;

	public Dependents Get(Dictionary<IEntityType, HashSet<IEntityType>> parameter)
	{
		var result = new Dependents();

		using var lease = _tarjan.Get(parameter)
		                         .AsValueEnumerable()
		                         .Select(g => g.ToList())
		                         .ToArray(ArrayPool<List<IEntityType>>.Shared);

		foreach (var g in lease)
		{
			result[g] = [];
			foreach (var type in g)
			{
				foreach (var dependencyGroup in parameter[type]
				                                .Select(x => lease.First(y => y.Contains(x)))
				                                .Where(y => y != g))
				{
					result[g].Add(dependencyGroup);
				}
			}
		}


		return result;
	}
}