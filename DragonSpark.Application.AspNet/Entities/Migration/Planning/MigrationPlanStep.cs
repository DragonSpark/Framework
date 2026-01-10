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