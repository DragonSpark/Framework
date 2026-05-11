using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class UpdateAwareMigrationSteps : IMigrationSteps
{
	readonly IMigrationSteps _previous;

	public UpdateAwareMigrationSteps(IMigrationSteps previous) => _previous = previous;

	public IEnumerable<IMigrationStep> Get(Array<IEntityMigrator> parameter)
	{
		var migrators = parameter.Open().OfType<IUpdateAwareEntityMigrator>().ToArray();
		foreach (var step in _previous.Get(parameter))
		{
			yield return step is IMigrationBody ? new UpdateAwareMigrationStep(step, migrators) : step;
		}
	}
}