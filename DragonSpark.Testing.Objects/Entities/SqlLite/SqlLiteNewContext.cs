using System;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DragonSpark.Testing.Objects.Entities.SqlLite;

[MustDisposeResource(false)]
public sealed class SqlLiteNewContext<T> : INewContext<T>, IAsyncDisposable where T : DbContext
{
	[MustDisposeResource(false)]
	public SqlLiteNewContext() : this(NewSqlLiteOptions<T>.Default.Get()) {}

	[MustDisposeResource(false)]
	public SqlLiteNewContext(DbContextOptions<T> options) : this(new NewContext<T>(new SqlLiteDbContexts<T>(options))) {}

	[MustDisposeResource(false)]
	public SqlLiteNewContext(INewContext<T> @new) => NewContext = @new;

	public INewContext<T> NewContext { get; }

	[MustDisposeResource]
	public async ValueTask<SqlLiteNewContext<T>> Initialize()
	{
		await using var context = NewContext.Get();
		if (!await context.Database.GetService<IDatabaseCreator>()
		                  .To<RelationalDatabaseCreator>()
		                  .ExistsAsync()
		                  .Await())
		{
			await context.Database.EnsureDeletedAsync().Await();
		}

		await context.Database.EnsureCreatedAsync().Await();
		return this;
	}

	[MustDisposeResource]
	public T Get() => NewContext.Get();

	public async ValueTask DisposeAsync()
	{
		await using var context = NewContext.Get();
		await context.Database.EnsureDeletedAsync().Await();
	}
}