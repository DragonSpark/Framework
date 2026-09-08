using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;

public sealed record SourceInput<T>(
	ILogger Logger,
	ushort PageSize,
	DbContext Source,
	IWorkspaces Workspaces,
	IQueryable<T> From,
	uint Total);