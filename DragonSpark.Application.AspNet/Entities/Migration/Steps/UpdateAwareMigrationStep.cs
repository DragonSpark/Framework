using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed class UpdateAwareMigrationStep : IMigrationBody
{
	readonly IMigrationStep                    _previous;
	readonly Array<IUpdateAwareEntityMigrator> _migrators;

	public UpdateAwareMigrationStep(IMigrationStep previous, Array<IUpdateAwareEntityMigrator> migrators)
	{
		_previous  = previous;
		_migrators = migrators;
	}

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, batchSize), stop) = parameter;
		await _previous.Off(parameter);
		var input = new UpdateEntityMigratorInput(logger, batchSize).Stop(stop);
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(input);
		}
	}
}