using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public class MigrationOrder : IArray<IModel, IEntityType>
{
	readonly ITopologicalSort _sort;

	protected MigrationOrder() : this(TopologicalSort.Default) {}

	public MigrationOrder(ITopologicalSort sort) => _sort = sort;

	public Array<IEntityType> Get(IModel parameter)
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

public interface ITopologicalSort : IArray<Lease<IEntityType>, IEntityType>;

public interface ITarjan : ISelect<Dictionary<IEntityType, HashSet<IEntityType>>, Entities>;

/// <summary>
/// ATTRIBUTION: Copilot
/// </summary>
// -------------------------------
// Tarjan SCC implementation
// -------------------------------

// -------------------------------
// Topological sort of SCC groups
// -------------------------------
sealed class Tarjan : ITarjan
{
	public static Tarjan Default { get; } = new();

	Tarjan() : this(EqualityComparer<IEntityType>.Default) {}

	readonly IEqualityComparer<IEntityType> _comparer;

	public Tarjan(IEqualityComparer<IEntityType> comparer) => _comparer = comparer;

	public Entities Get(Dictionary<IEntityType, HashSet<IEntityType>> parameter)
	{
		var index   = 0;
		var stack   = new Stack<IEntityType>();
		var indices = new Dictionary<IEntityType, int>();
		var lowlink = new Dictionary<IEntityType, int>();
		var on      = new HashSet<IEntityType>();
		var result  = new Entities();

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
			on.Add(v);

			foreach (var w in parameter[v])
			{
				if (!indices.TryGetValue(w, out var i))
				{
					StrongConnect(w);
					lowlink[v] = Math.Min(lowlink[v], lowlink[w]);
				}
				else if (on.Contains(w))
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
					on.Remove(w);
					scc.Add(w);
				} while (!_comparer.Equals(w, v));

				result.Add(scc);
			}
		}
	}
}

public sealed class Entities : List<List<IEntityType>>;

sealed class Dependencies : ISelect<IEntityType, HashSet<IEntityType>>
{
	public static Dependencies Default { get; } = new();

	Dependencies() {}

	public HashSet<IEntityType> Get(IEntityType parameter) => parameter.GetForeignKeys()
	                                                                   .Where(x => !x.IsOwnership)
	                                                                   .Select(x => x.PrincipalEntityType)
	                                                                   .Where(t => t.FindPrimaryKey() != null)
	                                                                   .SelectMany(x => x.GetDerivedTypes().Prepend(x))
	                                                                   .Where(x => !x.IsAbstract())
	                                                                   .ToHashSet();
}

sealed class TopologicalSort : ArraySelection<Lease<IEntityType>, IEntityType>, ITopologicalSort
{
	public static TopologicalSort Default { get; } = new();

	TopologicalSort() : this(ComposeGraph.Default, ComposeDependents.Default, ComposeEntities.Default) {}

	public TopologicalSort(IComposeGraph graph, IDetermineDependents dependents,
	                       IArray<Dependents, IEntityType> entities)
		: base(graph.Then().Select(dependents).Select(entities)) {}
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
			result[key].RemoveWhere(x => !result.ContainsKey(x));
		}

		return result;
	}
}

sealed class ComposeEntities : IArray<Dependents, IEntityType>
{
	public static ComposeEntities Default { get; } = new();

	ComposeEntities() : this(x => !x.IsAbstract()) {}

	readonly Func<IEntityType, bool> _where;

	public ComposeEntities(Func<IEntityType, bool> where) => _where = where;

	public Array<IEntityType> Get(Dependents parameter)
	{
		var entities  = new Entities();
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
				entities.Add(r);
				remaining.Remove(r);

				foreach (var d in remaining.Values)
				{
					d.Remove(r);
				}
			}
		}

		entities.AddRange(remaining.Keys);

		var result = entities.SelectMany(x => x.AsEnumerable().Reverse()).Where(_where).Result();
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

public readonly record struct RootedFromInput(Dictionary<IEntityType, HashSet<IEntityType>> Graph, IEntityType Root);

sealed class RootedFrom : ISelect<RootedFromInput, Dictionary<IEntityType, HashSet<IEntityType>>>
{
	public static RootedFrom Default { get; } = new();

	RootedFrom() {}

	public Dictionary<IEntityType, HashSet<IEntityType>> Get(RootedFromInput parameter)
	{
		var (graph, root) = parameter;
		var known = new HashSet<IEntityType>();
		var stack = new Stack<IEntityType>();
		stack.Push(root);
		while (stack.Count > 0)
		{
			var current = stack.Pop();
			if (!known.Add(current))
			{
				continue;
			}

			foreach (var dep in graph[current])
			{
				stack.Push(dep);
			}
		}

		return graph.Where(kvp => known.Contains(kvp.Key))
		            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Where(known.Contains).ToHashSet());
	}
}