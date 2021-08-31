using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Objects.Entities
{
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
}