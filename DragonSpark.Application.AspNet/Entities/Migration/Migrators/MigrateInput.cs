using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct MigrateInput(ILogger Logger, IWorkspaces Workspaces, Values Values);