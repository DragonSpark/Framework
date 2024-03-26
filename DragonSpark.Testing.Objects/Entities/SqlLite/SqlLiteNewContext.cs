using DragonSpark.Application.Entities;
using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Testing.Objects.Entities.SqlLite;

public sealed class SqlLiteNewContext<T> : INewContext<T>, IAsyncDisposable where T : DbContext
{
	public SqlLiteNewContext() : this(NewSqlLiteOptions<T>.Default.Get()) {}

	public SqlLiteNewContext(DbContextOptions<T> options) : this(new NewContext<T>(new SqlLiteDbContexts<T>(options))) {}

	public SqlLiteNewContext(INewContext<T> @new) => NewContext = @new;

	public INewContext<T> NewContext { get; }

	public async ValueTask<SqlLiteNewContext<T>> Initialize()
	{
		await using var context = NewContext.Get();
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

	public T Get() => NewContext.Get();

	public async ValueTask DisposeAsync()
	{
		await using var context = NewContext.Get();
		await context.Database.EnsureDeletedAsync();
	}
}