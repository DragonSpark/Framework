using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public interface IContexts<out T> : IResult<T> where T : DbContext {}

	public sealed class DbContexts<T> : IContexts<T> where T : DbContext
	{
		readonly IDbContextFactory<T> _factory;

		public DbContexts(IDbContextFactory<T> factory) => _factory = factory;

		public T Get() => _factory.CreateDbContext();
	}

	/*
	public readonly struct In<T> : IAsyncDisposable
	{
		public In(DbContext context, T parameter)
		{
			Context   = context;
			Parameter = parameter;
		}

		public DbContext Context { get; }

		public T Parameter { get; }

		public void Deconstruct(out DbContext context, out T parameter)
		{
			context   = Context;
			parameter = Parameter;
		}

		public ValueTask DisposeAsync() => Context.DisposeAsync();
	}
	*/

	/*public interface IQuery<T> : IResult<Query<T>> where T : class {}

	public readonly struct Query<T> : IAsyncDisposable where T : class
	{
		readonly DbContext _context;

		public Query(DbContext context) : this(context, context.Set<T>()) {}

		public Query(DbContext context, IQueryable<T> subject)
		{
			_context = context;
			Subject  = subject;
		}

		public Query<TTo> Select<TTo>(IQueryable<TTo> parameter) where TTo : class => new(_context, parameter);

		public IQueryable<T> Subject { get; }

		public In<TIn, T> In<TIn>(TIn parameter) => new(_context, Subject, parameter);

		public ValueTask DisposeAsync() => _context.DisposeAsync();

		public void Deconstruct(out DbContext context, out DbSet<T> subject)
		{
			context = _context;
			subject = _context.Set<T>();
		}
	}

	/*public class Root<TContext, T> : IQuery<T> where TContext : DbContext where T : class
	{
		readonly IDbContextFactory<TContext> _factory;

		protected Root(IDbContextFactory<TContext> factory) => _factory = factory;

		public Query<T> Get() => new(_factory.CreateDbContext());
	}#1#

	public class Adapter<TFrom, TTo> : IQuery<TTo> where TTo : class where TFrom : class
	{
		readonly IQuery<TFrom>               _previous;
		readonly ISelector<None, TFrom, TTo> _selector;

		public Adapter(IQuery<TFrom> previous, ISelector<None, TFrom, TTo> selector)
		{
			_previous = previous;
			_selector = selector;
		}

		public Query<TTo> Get()
		{
			var previous  = _previous.Get();
			var queryable = _selector.Get(previous.In(None.Default));
			var result    = previous.Select(queryable);
			return result;
		}
	}*/
}