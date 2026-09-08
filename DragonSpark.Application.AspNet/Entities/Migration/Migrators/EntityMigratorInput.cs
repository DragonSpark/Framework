using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct EntityMigratorInput(ILogger Logger, IWorkspaces Workspaces, ushort BatchSize)
{
	public EntityMigratorInput(ILogger logger, IWorkspaces workspaces)
		: this(logger, workspaces, DefaultBatchSize.Default) {}
}