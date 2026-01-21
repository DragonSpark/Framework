using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IUpdateAwareEntityMigrator : ICommand<UpdateEntityMigratorInput>;