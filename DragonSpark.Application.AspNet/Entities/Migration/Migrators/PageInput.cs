using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct PageInput<T>(
	ILogger Logger,
	IWorkspaces Workspaces,
	Func<DbContext, IQueryable<T>> Query,
	uint Size,
	uint Total,
	ConcurrentStack<uint> Results);