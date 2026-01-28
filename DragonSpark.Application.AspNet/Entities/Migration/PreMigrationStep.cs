using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class PreMigrationStep : IMigrationStep
{
	readonly Array<IEntityMigrator> _migrators;

	public PreMigrationStep(Array<IEntityMigrator> migrators) => _migrators = migrators;

	public async ValueTask Get(Stop<EntityMigratorInput> parameter)
	{
		var ((logger, _), stop) = parameter;
		var pre = new EntityPreMigrationInput(logger).Stop(stop);
		foreach (var migrator in _migrators.Open())
		{
			await migrator.Off(pre);
		}
	}
}