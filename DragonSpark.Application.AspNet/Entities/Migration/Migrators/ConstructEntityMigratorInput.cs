using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct ConstructEntityMigratorInput(
	IWorkspaceDefinition Definition,
	IEntityType From,
	IEntityType To);