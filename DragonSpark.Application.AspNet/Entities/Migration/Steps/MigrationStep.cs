using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed class MigrationStep : IMigrationBody
{
	readonly Array<IEntityMigrator> _migrators;

	public MigrationStep(Array<IEntityMigrator> migrators) => _migrators = migrators;

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(parameter);
		}
	}
}