using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class SupplementalStep : IMigrationStep
{
	readonly ISelect<Stop<EntityMigratorInput>, ValueTask> _previous;
	readonly ushort?                                       _batch;

	public SupplementalStep(ISelect<Stop<EntityMigratorInput>, ValueTask> previous, ushort? batch)
	{
		_previous = previous;
		_batch    = batch;
	}

	public ValueTask Get(Stop<EntityMigratorInput> parameter)
		=> _previous.Get(_batch is {} b
			                 ? parameter with { Subject = parameter.Subject with { BatchSize = b } }
			                 : parameter);
}