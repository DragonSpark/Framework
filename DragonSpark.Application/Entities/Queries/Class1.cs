using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	

	public interface IMaps : IAssignable<Type, IMap> {}

	public interface IMap : IAsyncDisposable
	{
		IQueryable<T> Query<T>() where T : class;
	}

	public interface ISession : IMaps, IAsyncDisposable {}

	sealed class Session : Assignable<Type, IMap>, ISession
	{
		readonly IEnumerable<IAsyncDisposable> _disposables;

		public Session() : this(new Dictionary<Type, IMap>()) {}

		public Session(IDictionary<Type, IMap> store) : this(store.Values, store.ToTable()) {}

		public Session(IEnumerable<IAsyncDisposable> disposables, ITable<Type, IMap> source) : base(source)
			=> _disposables = disposables;

		public async ValueTask DisposeAsync()
		{
			foreach (var context in _disposables.AsValueEnumerable())
			{
				await context.DisposeAsync();
			}
		}
	}

	public interface ISessions : IResult<ISession> {}

	sealed class Sessions : ISessions
	{
		public static Sessions Default { get; } = new Sessions();

		Sessions() {}

		public ISession Get() => new Session();
	}

	public interface IQueryMaps : IResult<IMap> {}

	public sealed class DbQueryMaps<T> : IQueryMaps where T : DbContext
	{
		readonly IDbContextFactory<T> _factory;

		public DbQueryMaps(IDbContextFactory<T> factory) => _factory = factory;

		public IMap Get() => new DbContextMap(_factory.CreateDbContext());
	}

	sealed class DbContextMap : IMap
	{
		readonly DbContext _context;

		public DbContextMap(DbContext context) => _context = context;

		public IQueryable<T> Query<T>() where T : class => _context.Set<T>();

		public ValueTask DisposeAsync() => _context.DisposeAsync();
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