using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class UpdateAwareMigrationStep : IMigrationBody
{
	readonly IMigrationStep                    _previous;
	readonly Array<IUpdateAwareEntityMigrator> _migrators;

	public UpdateAwareMigrationStep(IMigrationStep previous, Array<IEntityMigrator> migrators)
		: this(previous, migrators.Open().OfType<IUpdateAwareEntityMigrator>().ToArray()) {}

	public UpdateAwareMigrationStep(IMigrationStep previous, Array<IUpdateAwareEntityMigrator> migrators)
	{
		_previous  = previous;
		_migrators = migrators;
	}

	public void Execute(EntityMigratorInput parameter)
	{
		_previous.Execute(parameter);
		var (logger, batchSize) = parameter;
		var input = new UpdateEntityMigratorInput(logger, batchSize);
		foreach (var migrator in _migrators)
		{
			migrator.Execute(input);
		}
	}
}