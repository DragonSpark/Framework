using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class PostMigrationStep : IMigrationStep
{
	readonly Array<IEntityMigrator> _migrators;

	public PostMigrationStep(Array<IEntityMigrator> migrators) => _migrators = migrators;

	public void Execute(EntityMigratorInput parameter)
	{
		var pre = new EntityPostMigrationInput(parameter.Logger);
		foreach (var migrator in _migrators)
		{
			migrator.Execute(pre);
		}
	}
}