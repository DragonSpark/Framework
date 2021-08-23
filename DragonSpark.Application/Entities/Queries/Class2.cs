using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	class Class2 {}

	/*public interface IContextMap : IAssignable<Type, DbContext> {}*/

	/*public interface IMap : IAsyncDisposable
	{
		IQueryable<T> Query<T>() where T : class;
	}*/
	public interface IQuery<out T> : ISelect<DbContext, IQueryable<T>> {}

	/*public class DbQueryBase<TContext, T> : QueryBase<T> where TContext : DbContext where T : class
	{
		protected DbQueryBase(DbQuery<TContext, T> previous) : base(previous) {}

		protected DbQueryBase(DbQuery<TContext, T> previous, Func<IQueryable<T>, IQueryable<T>> query)
			: base(previous, query) {}

		protected DbQueryBase(IQuery<T> previous) : base(previous) {}
	}*/

	sealed class Root<T> : IQuery<T> where T : class
	{
		public static Root<T> Default { get; } = new Root<T>();

		Root() {}

		public IQueryable<T> Get(DbContext parameter) => parameter.Set<T>();
	}

	public class Append<T> : IQuery<T> where T : class
	{
		readonly IQuery<T>                          _previous;
		readonly Func<IQueryable<T>, IQueryable<T>> _select;

		public Append(IQuery<T> previous) : this(previous, x => x) {}

		public Append(Func<IQueryable<T>, IQueryable<T>> query) : this(Root<T>.Default, query) {}

		public Append(IQuery<T> previous, Func<IQueryable<T>, IQueryable<T>> select)
		{
			_previous = previous;
			_select   = select;
		}

		public IQueryable<T> Get(DbContext parameter) => _select(_previous.Get(parameter));
	}

	public interface ICompile<out T> : ISelect<DbContext, IAsyncEnumerable<T>> {}

	sealed class Compile<T> : Select<DbContext, IAsyncEnumerable<T>>, ICompile<T> where T : class
	{
		public Compile(IQuery<T> query) : this(context => query.Get(context)) {}

		public Compile(Expression<Func<DbContext, IQueryable<T>>> expression)
			: this(EF.CompileAsyncQuery(expression)) {}

		public Compile(Func<DbContext, IAsyncEnumerable<T>> select) : base(select) {}
	}

	/*public class Expression<TContext, T> : Select<TContext, IAsyncEnumerable<T>>
		where TContext : DbContext where T : class
	{
		public Expression(Expression<Func<TContext, IQueryable<T>>> expression)
			: this(EF.CompileAsyncQuery(expression)) {}

		public Expression(Func<TContext, IAsyncEnumerable<T>> select) : base(select) {}
	}*/

	/*public interface IQuery<TIn, out TOut> : ISelect<In<TIn>, IQueryable<TOut>> {}

	sealed class Start<T> : IQuery<T> where T : class
	{
		public static Start<T> Default { get; } = new Start<T>();

		Start() {}

		public IQueryable<T> Get(In<T> parameter) => parameter.Context.Set<T>();
	}*/

	/*
	public readonly struct In<TIn, T>
	{
		readonly DbContext _source;

		public In(DbContext source, IQueryable<T> query, TIn parameter)
		{
			_source   = source;
			Query     = query;
			Parameter = parameter;
		}

		public IQueryable<T> Query { get; }

		public TIn Parameter { get; }

		public void Deconstruct(out DbContext source, out IQueryable<T> query, out TIn parameter)
		{
			source    = _source;
			query     = Query;
			parameter = Parameter;
		}
	}

	public interface ISelector<TIn, TOut> : ISelector<TIn, TOut, TOut> {}

	public interface ISelector<TIn, T, out TOut> : ISelect<In<TIn, T>, IQueryable<TOut>> {}

	public class WhereSelector<TIn, T> : ISelector<TIn, T>
	{
		readonly Express<TIn, T> _select;

		public WhereSelector(Expression<Func<T, bool>> where) : this(where.Start().Accept<TIn>().Get().Get) {}

		public WhereSelector(Express<TIn, T> select) => _select = select;

		public IQueryable<T> Get(In<TIn, T> parameter) => parameter.Query.Where(_select(parameter.Parameter));
	}

	public class SelectSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom, TTo> _select;

		public SelectSelector(Expression<Func<TFrom, TTo>> select) : this(select.Start().Accept<TIn>().Get().Get) {}

		public SelectSelector(Express<TIn, TFrom, TTo> select) => _select = @select;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter) => parameter.Query.Select(_select(parameter.Parameter));
	}

	public class SelectManySelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom, IEnumerable<TTo>> _select;

		public SelectManySelector(Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: this(select.Start().Accept<TIn>().Get().Get) {}

		public SelectManySelector(Express<TIn, TFrom, IEnumerable<TTo>> select) => _select = @select;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> parameter.Query.SelectMany(_select(parameter.Parameter));
	}

	public class SelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Func<TIn, Func<IQueryable<TFrom>, IQueryable<TTo>>> _selection;

		public SelectionSelector(Func<IQueryable<TFrom>, IQueryable<TTo>> select)
			: this(Start.A.Result(select).Accept<TIn>()) {}

		public SelectionSelector(Func<TIn, Func<IQueryable<TFrom>, IQueryable<TTo>>> selection)
			=> _selection = selection;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter) => _selection(parameter.Parameter)(parameter.Query);
	}

	public class WhereManySelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>                       _where;
		readonly Expression<Func<TFrom, IEnumerable<TTo>>> _select;

		public WhereManySelector(Express<TIn, TFrom> where, Expression<Func<TFrom, IEnumerable<TTo>>> select)
		{
			_where  = @where;
			_select = @select;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> parameter.Query.Where(_where(parameter.Parameter)).SelectMany(_select);
	}

	public class WhereSelectSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>          _where;
		readonly Expression<Func<TFrom, TTo>> _select;

		public WhereSelectSelector(Express<TIn, TFrom> where, Expression<Func<TFrom, TTo>> select)
		{
			_where  = @where;
			_select = @select;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> parameter.Query.Where(_where(parameter.Parameter)).Select(_select);
	}

	public class WhereSelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>                      _where;
		readonly Func<IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public WhereSelectionSelector(Express<TIn, TFrom> where, Func<IQueryable<TFrom>, IQueryable<TTo>> selection)
		{
			_where     = where;
			_selection = selection;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> _selection(parameter.Query.Where(_where(parameter.Parameter)));
	}

	public class ParameterAwareSelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public ParameterAwareSelectionSelector(Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			=> _selection = selection;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter) => _selection(parameter.Parameter, parameter.Query);
	}

	public class ParameterAwareWhereSelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>                           _where;
		readonly Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public ParameterAwareWhereSelectionSelector(Express<TIn, TFrom> where,
		                                            Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
		{
			_where     = where;
			_selection = selection;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
		{
			var (_, queryable, @in) = parameter;
			var result = _selection(@in, queryable.Where(_where(@in)));
			return result;
		}
	}

	public class ExceptSelector<TIn, T> : ISelector<TIn, T> where T : class
	{
		readonly ISelector<TIn, T> _other;

		public ExceptSelector(ISelector<TIn, T> other) => _other = other;

		public IQueryable<T> Get(In<TIn, T> parameter)
		{
			var other  = _other.Get(parameter);
			var result = parameter.Query.Except(other);
			return result;
		}
	}*/
}