using DragonSpark.Application.Entities.Queries.Materialize;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public interface IQuery<T> : IResult<Query<T>> where T : class {}

	public readonly struct Query<T> : IAsyncDisposable where T : class
	{
		readonly IAsyncDisposable _previous;

		public Query(DbContext context) : this(context, context.Set<T>()) {}

		public Query(IAsyncDisposable previous, IQueryable<T> subject)
		{
			_previous = previous;
			Subject   = subject;
		}

		public Query<TTo> Select<TTo>(IQueryable<TTo> parameter) where TTo : class => new(_previous, parameter);

		public IQueryable<T> Subject { get; }

		public ValueTask DisposeAsync() => _previous.DisposeAsync();

		public void Deconstruct(out IQueryable<T> subject)
		{
			subject = Subject;
		}
	}

	public interface IProject<in TIn, out TOut> : ISelect<IQueryable<TIn>, IQueryable<TOut>> {}

	public class Root<TContext, T> : IQuery<T> where TContext : DbContext where T : class
	{
		readonly IDbContextFactory<TContext> _contexts;

		protected Root(IDbContextFactory<TContext> contexts) => _contexts = contexts;

		public Query<T> Get() => new(_contexts.CreateDbContext());
	}

	public readonly struct In<T, TKey>
	{
		public In(IQueryable<T> query, TKey parameter)
		{
			Query     = query;
			Parameter = parameter;
		}

		public IQueryable<T> Query { get; }

		public TKey Parameter { get; }

		public void Deconstruct(out IQueryable<T> query, out TKey parameter)
		{
			query     = Query;
			parameter = Parameter;
		}
	}

	public sealed class Accept<TIn, T> : DelegatedResult<TIn, Query<T>>, IQuery<TIn, T> where T : class
	{
		public Accept(IQuery<T> instance) : base(instance.Get) {}
	}

	public interface ISelector<TKey, TOut> : ISelector<TOut, TKey, TOut> {}

	public interface ISelector<T, TKey, out TOut> : ISelect<In<T, TKey>, IQueryable<TOut>> {}

	public sealed class WhereSelector<TKey, T> : ISelector<TKey, T>
	{
		readonly Express<TKey, T> _select;

		public WhereSelector(Express<TKey, T> select) => _select = select;

		public IQueryable<T> Get(In<T, TKey> parameter) => parameter.Query.Where(_select(parameter.Parameter));
	}

	public class Where<TIn, TOut> : Query<TIn, TOut> where TOut : class
	{
		protected Where(IQuery<TOut> query, Express<TIn, TOut> select)
			: base(query, new WhereSelector<TIn, TOut>(select)) {}

		protected Where(IQuery<TIn, TOut> query, Express<TIn, TOut> select)
			: base(query, new WhereSelector<TIn, TOut>(select)) {}
	}

	public interface IQuery<in TIn, TOut> : ISelect<TIn, Query<TOut>> where TOut : class {}

	public class Query<TIn, T> : Query<T, TIn, T> where T : class
	{
		protected Query(IQuery<T> query, ISelector<T, TIn, T> select) : base(query, select) {}

		protected Query(IQuery<TIn, T> query, ISelector<T, TIn, T> select) : base(query, select) {}
	}

	public class Query<T, TIn, TOut> : IQuery<TIn, TOut> where TOut : class where T : class
	{
		readonly IQuery<TIn, T>          _query;
		readonly ISelector<T, TIn, TOut> _select;

		protected Query(IQuery<T> query, ISelector<T, TIn, TOut> select) : this(new Accept<TIn, T>(query), select) {}

		protected Query(IQuery<TIn, T> query, ISelector<T, TIn, TOut> select)
		{
			_query  = query;
			_select = select;
		}

		public Query<TOut> Get(TIn parameter)
		{
			var session = _query.Get(parameter);
			var query   = _select.Get(new(session.Subject, parameter));
			var result  = session.Select(query);
			return result;
		}
	}

	public class Select<TIn, TOut, TResult> : ISelecting<TIn, TResult> where TOut : class
	{
		readonly IQuery<TIn, TOut>            _query;
		readonly IMaterializer<TOut, TResult> _materializer;

		public Select(IQuery<TIn, TOut> query, IMaterializer<TOut, TResult> materializer)
		{
			_query        = query;
			_materializer = materializer;
		}

		public async ValueTask<TResult> Get(TIn parameter)
		{
			await using var session = _query.Get(parameter);
			var             result  = await _materializer.Await(session.Subject);
			return result;
		}
	}

	public class ToArraySelection<TIn, T> : Select<TIn, T, Array<T>> where T : class
	{
		public ToArraySelection(IQuery<TIn, T> query) : base(query, DefaultToArray<T>.Default) {}
	}

}