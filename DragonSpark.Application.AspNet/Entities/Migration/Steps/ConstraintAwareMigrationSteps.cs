using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class ConstraintAwareMigrationSteps : IMigrationSteps
{
	readonly IMigrationSteps _previous;
	readonly DatabaseFacade  _facade;

	public ConstraintAwareMigrationSteps(IMigrationSteps previous, DatabaseFacade facade)
	{
		_previous = previous;
		_facade   = facade;
	}

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
	{
		foreach (var step in _previous.Get(parameter))
		{
			yield return step is IMigrationBody ? new ConstraintAwareMigrationStep(_facade, step) : step;
		}
	}
}