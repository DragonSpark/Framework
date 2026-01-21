using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class MigrationStep : IMigrationBody
{
	readonly Array<IEntityMigrator> _migrators;

	public MigrationStep(Array<IEntityMigrator> migrators) => _migrators = migrators;

	public void Execute(EntityMigratorInput parameter)
	{
		foreach (var migrator in _migrators)
		{
			migrator.Execute(parameter);
		}
	}
}