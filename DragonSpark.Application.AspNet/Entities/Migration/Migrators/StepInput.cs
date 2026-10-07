using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Workspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct StepInput<T>(
	ILogger Logger,
	IWorkspaces Workspaces,
	Func<DbContext, IQueryable<T>> Query,
	ushort Size,
	uint Total,
	IEntityProcessor<T> Processor,
	ConcurrentStack<uint> Results);