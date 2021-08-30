using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Execution;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Testing.Objects.Entities
{
	class Class1 {}

	public sealed class CounterAwareDbContexts<T> : IDbContextFactory<T> where T : DbContext
	{
		readonly IDbContextFactory<T> _previous;
		readonly ICounter             _counter;

		public CounterAwareDbContexts(IDbContextFactory<T> previous, ICounter counter)
		{
			_previous = previous;
			_counter  = counter;
		}

		public T CreateDbContext()
		{
			_counter.Execute();
			return _previous.CreateDbContext();
		}
	}

	public sealed class InMemoryDbContextFactory<T> : DbContextFactory<T> where T : DbContext
	{
		public InMemoryDbContextFactory() : this(IdentifyingText.Default.Get()) {}

		public InMemoryDbContextFactory(string name)
			: base(new DbContextOptionsBuilder<T>().UseInMemoryDatabase(name).Options) {}
	}

	public sealed class MemoryContexts<T> : DbContexts<T> where T : DbContext
	{
		public MemoryContexts() : this(IdentifyingText.Default.Get()) {}

		public MemoryContexts(string name) : base(new InMemoryDbContextFactory<T>(name)) {}
	}

	public sealed class SqlContexts<T> : IContexts<T>, IAsyncDisposable where T : DbContext
	{
		readonly IContexts<T> _contexts;

		public SqlContexts() : this($"{DefaultSqlDbName.Default}-{IdentifyingText.Default}") {}

		public SqlContexts(string name) : this(new DbContexts<T>(new SqlDbContexts<T>(name))) {}

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

	sealed class DefaultSqlDbName : Text.Text
	{
		public static DefaultSqlDbName Default { get; } = new DefaultSqlDbName();

		DefaultSqlDbName() : base("temporary.efcore.testing.db") {}
	}

	public sealed class SqlDbContexts<T> : DbContextFactory<T> where T : DbContext
	{
		public SqlDbContexts() : this(DefaultSqlDbName.Default) {}

		public SqlDbContexts(string name)
			: this(new DbContextOptionsBuilder<T>()
			       .UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database={name};Trusted_Connection=True;MultipleActiveResultSets=true")
			       .Options) {}

		public SqlDbContexts(DbContextOptions<T> options) : base(options) {}
	}

	public class DbContextFactory<T> : Result<T>, IDbContextFactory<T> where T : DbContext
	{
		protected DbContextFactory(DbContextOptions<T> options)
			: this(Start.A.Selection<DbContextOptions>().By.Instantiation<T>().Bind(options)) {}

		protected DbContextFactory(Func<T> create) : base(create) {}

		public T CreateDbContext() => Get();
	}
}