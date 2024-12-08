using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.AspNet.Compose.Entities;

sealed class DefaultContextFactory<T> : ISelect<IServiceProvider, T> where T : DbContext
{
	public static DefaultContextFactory<T> Default { get; } = new();

	DefaultContextFactory() {}

	public T Get(IServiceProvider parameter)
		=> parameter.GetRequiredService<IDbContextFactory<T>>().CreateDbContext();
}