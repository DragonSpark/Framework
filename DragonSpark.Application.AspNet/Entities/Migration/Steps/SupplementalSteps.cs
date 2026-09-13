using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class SupplementalSteps : IMigrationSteps
{
	readonly IMigrationSteps _previous;
	readonly IMigrationStep  _supplemental;

	public SupplementalSteps(IMigrationSteps previous, IEntityMigrator supplemental)
		: this(previous, new SupplementalStep(supplemental)) {}

	public SupplementalSteps(IMigrationSteps previous, IMigrationStep supplemental)
	{
		_previous     = previous;
		_supplemental = supplemental;
	}

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
		=> _previous.Get(parameter).Append(_supplemental);
}