using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

public sealed class MigrationPlan : ISelect<IModel, MigrationPlanResult>
{
	public static MigrationPlan Default { get; } = new();

	MigrationPlan() : this(ProcessDependents.Default, MigrationPlanStep.Default) {}

	readonly ICommand<ProcessDependentsInput> _process;
	readonly ICommand<MigrationPlanStepInput> _step;

	public MigrationPlan(ICommand<ProcessDependentsInput> process, ICommand<MigrationPlanStepInput> step)
	{
		_process = process;
		_step    = step;
	}

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
				_step.Execute(new(key, graph, type, references));
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