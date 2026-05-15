using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IUpdateAwareEntityMigrator : IStopAware<UpdateEntityMigratorInput>;