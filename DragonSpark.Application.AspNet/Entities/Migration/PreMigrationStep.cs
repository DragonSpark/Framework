using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class PreMigrationStep : IMigrationStep
{
	readonly Array<IEntityMigrator> _migrators;

	public PreMigrationStep(Array<IEntityMigrator> migrators) => _migrators = migrators;

	public void Execute(EntityMigratorInput parameter)
	{
		var pre = new EntityPreMigrationInput(parameter.Logger);
		foreach (var migrator in _migrators)
		{
			migrator.Execute(pre);
		}
	}
}