using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public class MigrationPlan : ISelect<IModel, MigrationPlanResult>
{
	readonly ITopologicalSort _sort;

	protected MigrationPlan() : this(TopologicalSort.Default) {}

	public MigrationPlan(ITopologicalSort sort) => _sort = sort;

	public MigrationPlanResult Get(IModel parameter)
	{
		using var entities = parameter.GetEntityTypes()
		                              .Where(t => !t.IsOwned() && !t.ClrType.IsAbstract && t.FindPrimaryKey() != null)
		                              .AsValueEnumerable()
		                              .ToArray(ArrayPool<IEntityType>.Shared);
		var result = _sort.Get(entities);
		return result;
	}
}

// TODO

public interface ITopologicalSort : ISelect<Lease<IEntityType>, MigrationPlanResult>;

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
		             .Where(x => x is { IsOwnership: false, IsRequired: true })
		             .Select(x => x.PrincipalEntityType)
		             .Where(t => t.FindPrimaryKey() != null && !t.ClrType.IsAbstract && !t.IsOwned())
		             .ToHashSet()) {}
}

sealed class TopologicalSort : ITopologicalSort
{
	public static TopologicalSort Default { get; } = new();

	TopologicalSort() : this(Dependencies.Default.Get, DetermineDependents.Default, ComposeCycles.Default) {}

	readonly Func<IEntityType, HashSet<IEntityType>> _dependencies;
	readonly IDetermineDependents                    _dependents;
	readonly ISelect<Dependents, Cycles>             _cycles;

	public TopologicalSort(Func<IEntityType, HashSet<IEntityType>> dependencies, IDetermineDependents dependents,
	                       ISelect<Dependents, Cycles> cycles)
	{
		_dependencies = dependencies;
		_dependents   = dependents;
		_cycles       = cycles;
	}

	public MigrationPlanResult Get(Lease<IEntityType> parameter)
	{
		var graph = parameter.ToDictionary(x => x, _dependencies);
		foreach (var key in graph.Keys)
		{
			graph[key].RemoveWhere(x => !graph.Keys.Contains(x));
		}

		var dependants = _dependents.Get(graph);
		var groups     = _cycles.Get(dependants);
		var resolved   = groups.Where(x => x.Count == 1).SelectMany(x => x).Result();
		var cycles     = groups.Where(x => x.Count > 1).OrderBy(x => x.Count).Select(x => x.Result()).Result();
		return new(resolved, cycles);
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
			                           .Where(x => x.Value.Count == 0)
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

		result.AddRange(remaining.Keys);

		return result;
	}
}

public sealed class Dependents : Dictionary<List<IEntityType>, HashSet<List<IEntityType>>>;

public interface IDetermineDependents : ISelect<Dictionary<IEntityType, HashSet<IEntityType>>, Dependents>;

sealed class DetermineDependents : IDetermineDependents
{
	public static DetermineDependents Default { get; } = new();

	DetermineDependents() : this(Tarjan.Default) {}

	readonly ITarjan _tarjan;

	public DetermineDependents(ITarjan tarjan) => _tarjan = tarjan;

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
				foreach (var item in parameter[type].Select(x => lease.First(y => y.Contains(x))).Where(y => y != g))
				{
					result[g].Add(item);
				}
			}
		}

		return result;
	}
}