using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class EmptyEntityMigrator : Instance<EntityTypeMapping>, IEntityMigrator
{
	public EmptyEntityMigrator(EntityTypeMapping instance) : base(instance) {}

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => ValueTask.CompletedTask;
}