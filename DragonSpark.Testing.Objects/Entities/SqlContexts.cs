using DragonSpark.Application.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class SqlContexts<T> : IContexts<T>, IAsyncDisposable where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public SqlContexts() : this(NewSqlOptions<T>.Default.Get()) {}

		public SqlContexts(DbContextOptions<T> options) : this(new Contexts<T>(new SqlDbContexts<T>(options))) {}

		public SqlContexts(IContexts<T> contexts) => _contexts = contexts;

		public async ValueTask<SqlContexts<T>> Initialize()
		{
			await using var context = _contexts.Get();
			await context.Database.EnsureDeletedAsync();
			await context.Database.EnsureCreatedAsync();
			return this;
		}

		public T Get() => _contexts.Get();

		public async ValueTask DisposeAsync()
		{
			await using var context = _contexts.Get();
			await context.Database.EnsureDeletedAsync();
		}
	}
}