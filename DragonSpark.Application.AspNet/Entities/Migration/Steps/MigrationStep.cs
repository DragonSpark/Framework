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
		var ((logger, _, _), _) = parameter;
		var migrators = _migrators.Open();
		for (var index = 0; index < migrators.Length; index++)
		{
			var migrator = migrators[index];
			logger.LogInformation("Executing {Step}/{Total}: {Name}", index.Next(), migrators.Length,
			                      migrator.GetType());
			await migrator.Off(parameter);
		}
	}
}