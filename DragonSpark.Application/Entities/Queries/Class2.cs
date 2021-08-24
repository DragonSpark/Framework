using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	class Class2 {}

	public interface IQuery<T> : IResult<Expression<Func<DbContext, IQueryable<T>>>> where T : class {}

	public class Query<T> : Instance<Expression<Func<DbContext, IQueryable<T>>>>, IQuery<T> where T : class
	{
		public Query(Expression<Func<DbContext, IQueryable<T>>> instance) : base(instance) {}
	}

	public sealed class Set<T> : Query<T> where T : class
	{
		public static Set<T> Default { get; } = new Set<T>();

		Set() : base(context => context.Set<T>()) {}
	}

	public interface IQuery<TIn, T> : IResult<Expression<Func<DbContext, TIn, IQueryable<T>>>> where T : class {}

	public class Query<TIn, T> : Instance<Expression<Func<DbContext, TIn, IQueryable<T>>>>, IQuery<TIn, T>
		where T : class
	{
		protected Query(Expression<Func<DbContext, TIn, IQueryable<T>>> instance) : base(instance) {}
	}

	public class Start<T> : Start<None, T> where T : class
	{
		public Start(Func<IQueryable<T>, IQueryable<T>> @select) : base(@select) {}

		public Start(Func<DbContext, IQueryable<T>, IQueryable<T>> @select) : base(@select) {}
	}

	public class Start<TIn, T> : Query<TIn, T> where T : class
	{
		public Start(Func<IQueryable<T>, IQueryable<T>> select) : base((context, @in) => select(context.Set<T>())) {}

		public Start(Func<DbContext, IQueryable<T>, IQueryable<T>> select)
			: base((context, queryable) => select(context, context.Set<T>())) {}

		public Start(Func<DbContext, TIn, IQueryable<T>, IQueryable<T>> select)
			: base((context, @in) => select(context, @in, context.Set<T>())) {}

		protected Start(Expression<Func<DbContext, TIn, IQueryable<T>>> instance) : base(instance) {}
	}

	public interface ISelector<TIn, out T> : ISelect<In<TIn>, IQueryable<T>> {}

	public class Selected<TIn, T> : Start<TIn, T> where T : class
	{
		public Selected(Func<In<TIn>, IQueryable<T>> select)
			: base((context, @in) => select(new In<TIn>(context, @in))) {}
	}

	/*public class Append<TIn, T> : IQuery<TIn, T> where T : class
	{
		readonly IQuery<TIn, T>                     _previous;
		readonly Func<IQueryable<T>, IQueryable<T>> _select;

		protected Append(IQuery<TIn, T> previous) : this(previous, x => x) {}

		protected Append(IQuery<TIn, T> previous, Func<IQueryable<T>, IQueryable<T>> select)
		{
			_previous = previous;
			_select   = select;
		}

		public IQueryable<T> Get(In<TIn, T> parameter)
		{
			var previous = _previous.Get(parameter);
			var result   = _select(previous);
			return result;
		}
	}*/

	/*
	public sealed class Set<TIn, T> : IQuery<TIn, T> where T : class
	{
		public static Set<TIn, T> Default { get; } = new Set<TIn, T>();

		Set() {}

		public IQueryable<T> Get(In<TIn> parameter) => parameter.Context.Set<T>();
	}
	*/

	public readonly record struct In<T>(DbContext Context, T Parameter);
	/*
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