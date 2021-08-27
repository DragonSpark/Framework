using DragonSpark.Model;
using DragonSpark.Model.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Class3 {}

	public readonly record struct In<T>(DbContext Context, T Parameter);

	public interface IForm<TIn, out T> : ISelect<In<TIn>, IAsyncEnumerable<T>> {}

	sealed class Form<T> : IForm<None, T>
	{
		readonly Func<DbContext, IAsyncEnumerable<T>> _select;

		public Form(IQuery<T> query) : this(query.Get().Expand()) {}

		public Form(Expression<Func<DbContext, IQueryable<T>>> expression) : this(EF.CompileAsyncQuery(expression)) {}

		public Form(Func<DbContext, IAsyncEnumerable<T>> select) => _select = select;

		public IAsyncEnumerable<T> Get(In<None> parameter) => _select(parameter.Context);
	}

	sealed class Form<TIn, T> : IForm<TIn, T>
	{
		readonly Func<DbContext, TIn, IAsyncEnumerable<T>> _select;

		public Form(IQuery<TIn, T> query) : this(query.Get().Expand()) {}

		public Form(Expression<Func<DbContext, TIn, IQueryable<T>>> expression) :
			this(EF.CompileAsyncQuery(expression)) {}

		public Form(Func<DbContext, TIn, IAsyncEnumerable<T>> select) => _select = select;

		public IAsyncEnumerable<T> Get(In<TIn> parameter) => _select(parameter.Context, parameter.Parameter);
	}

	public readonly struct Invocation<T> : IAsyncDisposable, IDisposable
	{
		readonly IAsyncDisposable _disposable;

		public Invocation(IAsyncDisposable disposable, IAsyncEnumerable<T> elements)
		{
			_disposable = disposable;
			Elements    = elements;
		}

		public IAsyncEnumerable<T> Elements { get; }

		public ValueTask DisposeAsync() => _disposable.DisposeAsync();

		public void Dispose() {}
	}

	public interface IInvoke<in TIn, T> : ISelect<TIn, Invocation<T>> {}

	public class Invoke<TContext, T> : Invoke<TContext, None, T> where TContext : DbContext
	{
		public Invoke(IContexts<TContext> contexts, IQuery<T> query) : base(contexts, new Form<T>(query)) {}

		public Invoke(IContexts<TContext> contexts, IForm<None, T> form) : base(contexts, form) {}
	}

	public class Invoke<TContext, TIn, T> : IInvoke<TIn, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IForm<TIn, T>       _form;

		public Invoke(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, new Form<TIn, T>(query)) {}

		public Invoke(IContexts<TContext> contexts, IForm<TIn, T> form)
		{
			_contexts = contexts;
			_form     = form;
		}

		public Invocation<T> Get(TIn parameter)
		{
			var context = _contexts.Get();
			var form    = _form.Get(new(context, parameter));
			return new(context, form);
		}
	}

	/*public interface IQuery<in TIn, TOut> : ISelect<TIn, Query<TOut>> where TOut : class {}

	public class Query<TIn, T> : Query<TIn, T, T> where T : class
	{
		public Query(IQuery<T> query, ISelector<TIn, T, T> select) : base(query, select) {}

		public Query(IQuery<TIn, T> query, ISelector<TIn, T, T> select) : base(query, select) {}
	}

	public class Query<TIn, T, TOut> : IQuery<TIn, TOut> where TOut : class where T : class
	{
		readonly IQuery<TIn, T>          _query;
		readonly ISelector<TIn, T, TOut> _select;

		public Query(IQuery<T> query, ISelector<TIn, T, TOut> select) : this(new Accept<TIn, T>(query), select) {}

		public Query(IQuery<TIn, T> query, ISelector<TIn, T, TOut> select)
		{
			_query  = query;
			_select = select;
		}

		public Query<TOut> Get(TIn parameter)
		{
			var session = _query.Get(parameter);
			var query   = _select.Get(session.In(parameter));
			var result  = session.Select(query);
			return result;
		}
	}

	public sealed class Accept<TIn, T> : DelegatedResult<TIn, Query<T>>, IQuery<TIn, T> where T : class
	{
		public Accept(IQuery<T> instance) : base(instance.Get) {}
	}

	public class Where<TIn, TOut> : Query<TIn, TOut> where TOut : class
	{
		protected Where(IQuery<TOut> query, Express<TIn, TOut> select)
			: base(query, new WhereSelector<TIn, TOut>(select)) {}

		protected Where(IQuery<TIn, TOut> query, Express<TIn, TOut> select)
			: base(query, new WhereSelector<TIn, TOut>(select)) {}
	}

	public class WhereMany<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TFrom : class where TTo : class
	{
		protected WhereMany(IQuery<TFrom> query, Express<TIn, TFrom> where,
		                    Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(query, new WhereManySelector<TIn, TFrom, TTo>(where, select)) {}

		protected WhereMany(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where,
		                    Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(query, new WhereManySelector<TIn, TFrom, TTo>(where, select)) {}
	}

	public class WhereSelect<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TFrom : class where TTo : class
	{
		protected WhereSelect(IQuery<TFrom> query, Express<TIn, TFrom> where, Expression<Func<TFrom, TTo>> select)
			: base(query, new WhereSelectSelector<TIn, TFrom, TTo>(where, select)) {}

		protected WhereSelect(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where, Expression<Func<TFrom, TTo>> select)
			: base(query, new WhereSelectSelector<TIn, TFrom, TTo>(where, select)) {}
	}

	public class WhereSelection<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TFrom : class where TTo : class
	{
		protected WhereSelection(IQuery<TFrom> query, Express<TIn, TFrom> where,
		                         Func<IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new WhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}

		protected WhereSelection(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where,
		                         Func<IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new WhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}
	}

	public class ParameterAwareSelection<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TTo : class where TFrom : class
	{
		protected ParameterAwareSelection(IQuery<TFrom> query, Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareSelectionSelector<TIn, TFrom, TTo>(selection)) {}

		protected ParameterAwareSelection(IQuery<TIn, TFrom> query,
		                                  Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareSelectionSelector<TIn, TFrom, TTo>(selection)) {}
	}

	public class ParameterAwareWhereSelection<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo>
		where TTo : class where TFrom : class
	{
		protected ParameterAwareWhereSelection(IQuery<TFrom> query, Express<TIn, TFrom> where,
		                                       Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareWhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}

		protected ParameterAwareWhereSelection(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where,
		                                       Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareWhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}
	}*/
}