using System;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DragonSpark.Testing.Objects.Entities.Sql;

[UsedImplicitly, MustDisposeResource]
public sealed class SqlNewContext<T> : INewContext<T>, IAsyncDisposable where T : DbContext
{
	readonly INewContext<T> _new;

	public SqlNewContext() : this(NewSqlOptions<T>.Default.Get()) {}

	public SqlNewContext(DbContextOptions<T> options) : this(new NewContext<T>(new SqlDbContexts<T>(options))) {}

	public SqlNewContext(INewContext<T> @new) => _new = @new;

	[MustDisposeResource]
	public async ValueTask<SqlNewContext<T>> Initialize()
	{
		await using var context = _new.Get();
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
	public T Get() => _new.Get();

	public async ValueTask DisposeAsync()
	{
		await using var context = _new.Get();
		await context.Database.EnsureDeletedAsync().Await();
	}
}