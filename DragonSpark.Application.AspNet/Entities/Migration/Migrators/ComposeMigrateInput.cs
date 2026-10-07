using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct ComposeMigrateInput<T>(
	Func<DbContext, IQueryable<T>> Query,
	IEntityProcessor<T> Processor);