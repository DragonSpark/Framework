using DragonSpark.Application.Entities.Queries.Materialization;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	public interface IQuery<T> : IResult<QuerySession<T>> where T : class {}

	public readonly struct QuerySession<T> : IAsyncDisposable where T : class
	{
		readonly IAsyncDisposable _previous;

		public QuerySession(DbContext context) : this(context, context.Set<T>()) {}

		public QuerySession(IAsyncDisposable previous, IQueryable<T> subject)
		{
			_previous = previous;
			Subject   = subject;
		}

		public QuerySession<TTo> Select<TTo>(IQueryable<TTo> parameter) where TTo : class => new(_previous, parameter);

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

		public QuerySession<T> Get() => new(_contexts.CreateDbContext());
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

	public sealed class Accept<TIn, T> : DelegatedResult<TIn, QuerySession<T>>, ISession<TIn, T> where T : class
	{
		public Accept(IQuery<T> instance) : base(instance.Get) {}
	}

	public interface ISelector<TKey, TOut> : ISelector<TOut, TKey, TOut> {}

	public interface ISelector<T, TKey, out TOut> : ISelect<In<T, TKey>, IQueryable<TOut>> {}

	public sealed class WhereSelector<TKey, T> : ISelector<TKey, T>
	{
		readonly Query<TKey, T> _select;

		public WhereSelector(Query<TKey, T> select) => _select = select;

		public IQueryable<T> Get(In<T, TKey> parameter) => parameter.Query.Where(_select(parameter.Parameter));
	}

	public class Where<TIn, TOut> : Session<TIn, TOut> where TOut : class
	{
		protected Where(IQuery<TOut> query, Query<TIn, TOut> select)
			: base(query, new WhereSelector<TIn, TOut>(select)) {}

		protected Where(ISession<TIn, TOut> session, Query<TIn, TOut> select)
			: base(session, new WhereSelector<TIn, TOut>(select)) {}
	}

	public interface ISession<in TIn, TOut> : ISelect<TIn, QuerySession<TOut>> where TOut : class {}

	public class Session<TIn, T> : Session<T, TIn, T> where T : class
	{
		protected Session(IQuery<T> query, ISelector<T, TIn, T> select) : base(query, select) {}

		protected Session(ISession<TIn, T> session, ISelector<T, TIn, T> select) : base(session, select) {}
	}

	public class Session<T, TIn, TOut> : ISession<TIn, TOut> where TOut : class where T : class
	{
		readonly ISession<TIn, T>        _session;
		readonly ISelector<T, TIn, TOut> _select;

		protected Session(IQuery<T> query, ISelector<T, TIn, TOut> select) : this(new Accept<TIn, T>(query), select) {}

		protected Session(ISession<TIn, T> session, ISelector<T, TIn, TOut> select)
		{
			_session = session;
			_select  = select;
		}

		public QuerySession<TOut> Get(TIn parameter)
		{
			var session = _session.Get(parameter);
			var query   = _select.Get(new(session.Subject, parameter));
			var result  = session.Select(query);
			return result;
		}
	}

	sealed class Select<TIn, TOut, TResult> : ISelecting<TIn, TResult> where TOut : class
	{
		readonly ISession<TIn, TOut>          _session;
		readonly IMaterializer<TOut, TResult> _materializer;

		public Select(ISession<TIn, TOut> session, IMaterializer<TOut, TResult> materializer)
		{
			_session      = session;
			_materializer = materializer;
		}

		public async ValueTask<TResult> Get(TIn parameter)
		{
			await using var session = _session.Get(parameter);
			var             result  = await _materializer.Await(session.Subject);
			return result;
		}
	}
}