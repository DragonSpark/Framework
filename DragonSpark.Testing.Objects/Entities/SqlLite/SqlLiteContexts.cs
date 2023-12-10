using DragonSpark.Application.Entities;
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Testing.Objects.Entities.SqlLite;

public sealed class SqlLiteContexts<T> : IContexts<T>, IAsyncDisposable where T : DbContext
{
	public SqlLiteContexts() : this(NewSqlLiteOptions<T>.Default.Get()) {}

	public SqlLiteContexts(DbContextOptions<T> options) : this(new Contexts<T>(new SqlLiteDbContexts<T>(options))) {}

	public SqlLiteContexts(IContexts<T> contexts) => Contexts = contexts;

	public IContexts<T> Contexts { get; }

	public async ValueTask<SqlLiteContexts<T>> Initialize()
	{
		await using var context = Contexts.Get();
		if (!await context.Database.GetService<IDatabaseCreator>()
		                  .To<RelationalDatabaseCreator>()
		                  .ExistsAsync()
		                  .ConfigureAwait(false))
		{
			await context.Database.EnsureDeletedAsync().ConfigureAwait(false);
		}

		await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
		return this;
	}

	public T Get() => Contexts.Get();

	public async ValueTask DisposeAsync()
	{
		await using var context = Contexts.Get();
		await context.Database.EnsureDeletedAsync();
	}
}