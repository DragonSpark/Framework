using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

class Class2;

public readonly record struct MigrationPlanResult(
	IReadOnlyCollection<IEntityType> Resolved,
	IReadOnlyCollection<Depending> Unresolved);

public readonly record struct Depending(IEntityType Type, ushort Dependents);

public sealed class MigrationPlan : ISelect<IModel, MigrationPlanResult>
{
	public static MigrationPlan Default { get; } = new();

	MigrationPlan() : this(ProcessDependents.Default) {}

	readonly ICommand<ProcessDependentsInput> _process;

	public MigrationPlan(ICommand<ProcessDependentsInput> process) => _process = process;

	public MigrationPlanResult Get(IModel parameter)
	{
		var graph = new StandardTable<IEntityType, HashSet<IEntityType>>(_ => []);
		using var lease = parameter.GetEntityTypes()
		                           .Where(t => !t.ClrType.IsAbstract)
		                           .AsValueEnumerable()
		                           .ToArray(ArrayPool<IEntityType>.Shared);
		var references = lease.ToDictionary(x => x, _ => new HashSet<IEntityType>());
		foreach (var type in lease)
		{
			foreach (var key in type.GetForeignKeys().Where(x => x.IsRequired))
			{
				var principal = key.PrincipalEntityType;
				using var types = principal.GetDerivedTypes()
				                           .Where(d => !d.ClrType.IsAbstract)
				                           .AsValueEnumerable()
				                           .ToArray(ArrayPool<IEntityType>.Shared);
				if (types.Any())
				{
					foreach (var d in types)
					{
						if (graph.Get(d).Add(type))
						{
							references[type].Add(d);
						}
					}
				}
				else
				{
					if (graph.Get(principal).Add(type))
					{
						references[type].Add(principal);
					}
				}
			}
		}

		var result = new List<IEntityType>(lease.Length);
		_process.Execute(new(result, graph, references));

		var unresolved = references.Where(x => x.Value.Count > 0)
		                           .OrderBy(x => x.Value.Count)
		                           .Select(x => new Depending(x.Key, (ushort)x.Value.Count))
		                           .ToArray();
		return new(result.AsReadOnly(), unresolved);
	}
}

public readonly record struct ProcessDependentsInput(
	List<IEntityType> result,
	StandardTable<IEntityType, HashSet<IEntityType>> graph,
	Dictionary<IEntityType, HashSet<IEntityType>> indegree);

sealed class ProcessDependents : ICommand<ProcessDependentsInput>
{
	public static ProcessDependents Default { get; } = new();

	ProcessDependents() {}

	public void Execute(ProcessDependentsInput parameter)
	{
		var (result, graph, references) = parameter;
		var entityTypes = references.Where(x => x.Value.Count == 0).Select(x => x.Key).Except(result);
		var queue       = new Queue<IEntityType>(entityTypes);
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			result.Add(current);

			foreach (var dependent in graph.Get(current).OrderBy(x => references[x].Count))
			{
				var types = references[dependent];
				types.ExceptWith(result);
				switch (types.Count)
				{
					case 0:
						queue.Enqueue(dependent);
						break;
				}
			}
		}
	}
}

public readonly record struct DestinationModelCheckerInput(IReadOnlyCollection<IEntityType> Types, IModel Destination);

public sealed class DestinationModelChecker : ISelect<DestinationModelCheckerInput, DestinationModelResult>
{
	public static DestinationModelChecker Default { get; } = new();

	DestinationModelChecker() : this(IsExact.Default) {}

	readonly ICondition<IsExactInput> _exact;

	public DestinationModelChecker(ICondition<IsExactInput> exact) => _exact = exact;

	public DestinationModelResult Get(DestinationModelCheckerInput parameter)
	{
		var (types, destination) = parameter;

		var entities  = destination.GetEntityTypes().ToDictionary(t => t.Name);
		var exact     = new List<IEntityType>();
		var differing = new List<IEntityType>();
		var missing   = new List<IEntityType>();

		foreach (var from in types)
		{
			var collection = entities.TryGetValue(from.Name, out var to)
				                 ? _exact.Get(new(from, to)) ? exact : differing
				                 : missing;
			collection.Add(from);
		}

		return new(new(exact.AsReadOnly(), differing.AsReadOnly()), missing.AsReadOnly());
	}
}

public readonly record struct IsExactInput(IEntityType Source, IEntityType Destination);

sealed class IsExact : ICondition<IsExactInput>
{
	public static IsExact Default { get; } = new();

	IsExact() : this(HashSet<string>.CreateSetComparer()) {}

	readonly IEqualityComparer<HashSet<string>> _comparer;

	public IsExact(IEqualityComparer<HashSet<string>> comparer) => _comparer = comparer;

	public bool Get(IsExactInput parameter)
	{
		var (source, destination) = parameter;

		// Property names set
		var properties = source.GetProperties()
		                       .Select(p => p.Name)
		                       .ToHashSet()
		                       .SetEquals(destination.GetProperties().Select(p => p.Name).ToHashSet());
		return properties
		       /*&& destination.GetKeys()
		                     .Select(k => k.Properties.Select(p => p.Name).ToHashSet())
		                     .ToHashSet(_comparer)
		                     .SetEquals(source.GetKeys()
		                                      .Select(k => k.Properties.Select(p => p.Name).ToHashSet())
		                                      .ToHashSet(_comparer))
		       && source.GetIndexes()
		                .Select(i => i.Properties.Select(p => p.Name).ToHashSet())
		                .ToHashSet(_comparer)
		                .SetEquals(destination.GetIndexes()
		                                      .Select(i => i.Properties.Select(p => p.Name).ToHashSet())
		                                      .ToHashSet(_comparer))*/
		       && source.GetNavigations()
		                .Select(n => n.Name)
		                .ToHashSet()
		                .SetEquals(destination.GetNavigations().Select(n => n.Name).ToHashSet());
	}
}

public readonly record struct FoundModelsResult(
	IReadOnlyCollection<IEntityType> Exact,
	IReadOnlyCollection<IEntityType> Modified);

public readonly record struct DestinationModelResult(
	FoundModelsResult Found,
	IReadOnlyCollection<IEntityType> Missing);

public sealed class VerifyMigrationPlan : ISelect<IReadOnlyCollection<IEntityType>, IReadOnlyCollection<string>>
{
	public static VerifyMigrationPlan Default { get; } = new();

	VerifyMigrationPlan() {}

	public IReadOnlyCollection<string> Get(IReadOnlyCollection<IEntityType> parameter)
	{
		using var lease  = parameter.AsValueEnumerable().ToArray(ArrayPool<IEntityType>.Shared);
		var       span   = lease.Memory.Span;
		var       result = new List<string>();
		for (var i = 0; i < lease.Length; i++)
		{
			var current = span[i];

			foreach (var fk in current.GetForeignKeys().Where(f => f.IsRequired))
			{
				var principal = fk.PrincipalEntityType;
				using var types = principal.GetDerivedTypes()
				                           .AsValueEnumerable()
				                           .ToArray(ArrayPool<IEntityType>.Shared);
				var checkTypes = types.Any() ? types.Where(d => !d.ClrType.IsAbstract) : [principal];

				foreach (var check in checkTypes)
				{
					var index = span.IndexOf(check);
					if (index == -1)
					{
						result.Add($"[{i}] {current.GetTableName()} depends on missing {check.GetTableName()}");
					}
					else if (index >= i)
					{
						result.Add($"[{i}] {current.GetTableName()} depends on later [{index}] {check.GetTableName()} — VIOLATION RISK");
					}
				}
			}
		}

		return result;
	}
}