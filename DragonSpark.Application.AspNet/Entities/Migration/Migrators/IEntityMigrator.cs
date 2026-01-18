using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IEntityMigrator : ICommand<EntityMigratorInput>, IResult<EntityTypeMapping>;