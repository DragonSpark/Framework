using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IEntityMigrators : IArray<IWorkspaceDefinition, IEntityMigrator>;