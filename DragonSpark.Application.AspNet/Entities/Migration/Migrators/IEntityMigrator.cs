using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IEntityMigrator : IStopAware<EntityPreMigrationInput>,
								   IStopAware<EntityPostMigrationInput>,
								   IStopAware<EntityMigratorInput>,
								   IResult<EntityTypeMapping>;