using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using Microsoft.EntityFrameworkCore;
using System;

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

	public class InMemoryDbContexts<T> : IDbContextFactory<T> where T : DbContext
	{
		readonly Func<T> _create;

		public InMemoryDbContexts()
			: this(Start.A.Selection<DbContextOptions>().By.Instantiation<T>(), IdentifyingText.Default.Get()) {}

		public InMemoryDbContexts(DragonSpark.Compose.Model.Selection.Selector<DbContextOptions, T> create,
		                          string name)
			: this(create, new DbContextOptionsBuilder<T>().UseInMemoryDatabase(name).Options) {}

		public InMemoryDbContexts(DragonSpark.Compose.Model.Selection.Selector<DbContextOptions, T> create,
		                          DbContextOptions options)
			: this(create.Bind(options)) {}

		public InMemoryDbContexts(Func<T> create) => _create = create;

		public T CreateDbContext() => _create();
	}
}