using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public sealed class PostMigrationStep : IMigrationStep
{
	readonly Array<IEntityMigrator> _migrators;

	public PostMigrationStep(Array<IEntityMigrator> migrators) => _migrators = migrators;

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, _), stop) = parameter;
		var pre = new EntityPostMigrationInput(logger).Stop(stop);
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(pre);
		}
	}
}