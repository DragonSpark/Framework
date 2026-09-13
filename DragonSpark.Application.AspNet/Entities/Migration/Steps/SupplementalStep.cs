using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class SupplementalStep : StopAware<EntityMigratorInput>, IMigrationStep
{
	public SupplementalStep(ISelect<Stop<EntityMigratorInput>, ValueTask> previous) : base(previous) {}
}