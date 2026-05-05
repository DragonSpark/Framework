using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IEntityMigrator : IStopAware<EntityPreMigrationInput>,
                                   IStopAware<EntityPostMigrationInput>,
                                   IStopAware<EntityMigratorInput>,
                                   IResult<EntityTypeMapping>;

// TODO V2

public sealed class EmptyEntityMigrator : Instance<EntityTypeMapping>, IEntityMigrator
{
	public EmptyEntityMigrator(EntityTypeMapping instance) : base(instance) {}

	public ValueTask Get(Stop<EntityPreMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityPostMigrationInput> parameter) => ValueTask.CompletedTask;

	public ValueTask Get(Stop<EntityMigratorInput> parameter) => ValueTask.CompletedTask;
}