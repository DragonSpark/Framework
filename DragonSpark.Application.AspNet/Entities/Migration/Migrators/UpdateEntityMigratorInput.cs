using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct UpdateEntityMigratorInput(
	ILogger Logger,
	IWorkspaceDefinition Definition,
	ushort BatchSize);