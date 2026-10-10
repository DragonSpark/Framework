using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public sealed record PageInput<T>(ILogger Logger, Workspace Workspace, Array<T> From, uint Total);