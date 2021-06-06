using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class DefaultContextFactory<T> : ISelect<IServiceProvider, T> where T : DbContext
	{
		public static DefaultContextFactory<T> Default { get; } = new DefaultContextFactory<T>();

		DefaultContextFactory() {}

		public T Get(IServiceProvider parameter)
			=> parameter.GetRequiredService<IDbContextFactory<T>>().CreateDbContext();
	}
}