using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

public sealed record SourceInput<T>(
	ILogger Logger,
	Workspace Workspace,
	IQueryable<T> Source,
	uint Start,
	uint Size,
	uint Total);