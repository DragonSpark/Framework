using DragonSpark.Application.AspNet.Entities.Migration.Migrators;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public interface IMigrationStep : ICommand<EntityMigratorInput>;