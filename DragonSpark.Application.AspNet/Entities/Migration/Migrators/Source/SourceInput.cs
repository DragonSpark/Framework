using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

public sealed record SourceInput<T>(
	ILogger Logger,
	IEntities Entities,
	IQueryable<T> Source,
	ushort PageSize,
	uint Total);