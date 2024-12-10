using System;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Compose.Entities;

sealed class DefaultContextFactory<T> : ISelect<IServiceProvider, T> where T : DbContext
{
	public static DefaultContextFactory<T> Default { get; } = new();

	DefaultContextFactory() {}

	[MustDisposeResource(false)]
	public T Get(IServiceProvider parameter)
		=> parameter.GetRequiredService<IDbContextFactory<T>>().CreateDbContext();
}
