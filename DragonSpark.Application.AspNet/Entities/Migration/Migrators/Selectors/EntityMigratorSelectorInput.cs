using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

public readonly record struct EntityMigratorSelectorInput(IWorkspaceDefinition Definition, EntityComparisonResult Result);