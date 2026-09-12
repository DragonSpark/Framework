using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct EntityMigratorInput(ILogger Logger, IWorkspaceDefinition Workspaces, ushort BatchSize);