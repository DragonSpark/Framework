using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Metadata;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class MigrationPlanStep : ICommand<MigrationPlanStepInput>
{
	public static MigrationPlanStep Default { get; } = new();

	MigrationPlanStep() {}

	public void Execute(MigrationPlanStepInput parameter)
	{
		var (key, graph, type, references) = parameter;
		var principal = key.PrincipalEntityType;
		using var types = principal.GetDerivedTypes()
		                           .Append(principal)
		                           .Where(d => !d.ClrType.IsAbstract)
		                           .AsValueEnumerable()
		                           .Distinct()
		                           .ToArray(ArrayPool<IEntityType>.Shared);
		foreach (var d in types)
		{
			if (graph.Get(d).Add(type))
			{
				references[type].Add(d);
			}
		}
	}
}