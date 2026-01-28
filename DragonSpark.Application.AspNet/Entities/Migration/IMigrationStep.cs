using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public interface IMigrationStep : IStopAware<EntityMigratorInput>;