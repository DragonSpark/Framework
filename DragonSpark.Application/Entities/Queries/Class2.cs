using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	class Class2 {}

	public readonly struct In<T, TIn>
	{
		public In(IQueryable<T> query, TIn parameter)
		{
			Query     = query;
			Parameter = parameter;
		}

		public IQueryable<T> Query { get; }

		public TIn Parameter { get; }

		public void Deconstruct(out IQueryable<T> query, out TIn parameter)
		{
			query     = Query;
			parameter = Parameter;
		}
	}

	public interface ISelector<TIn, TOut> : ISelector<TOut, TIn, TOut> {}

	public interface ISelector<T, TIn, out TOut> : ISelect<In<T, TIn>, IQueryable<TOut>> {}

	public class WhereSelector<TIn, T> : ISelector<TIn, T>
	{
		readonly Express<TIn, T> _select;

		public WhereSelector(Express<TIn, T> select) => _select = select;

		public IQueryable<T> Get(In<T, TIn> parameter) => parameter.Query.Where(_select(parameter.Parameter));
	}

	public class WhereManySelector<TIn, TFrom, TTo> : ISelector<TFrom, TIn, TTo>
	{
		readonly Express<TIn, TFrom>                       _where;
		readonly Expression<Func<TFrom, IEnumerable<TTo>>> _select;

		public WhereManySelector(Express<TIn, TFrom> @where, Expression<Func<TFrom, IEnumerable<TTo>>> select)
		{
			_where  = @where;
			_select = select;
		}

		public IQueryable<TTo> Get(In<TFrom, TIn> parameter)
			=> parameter.Query.Where(_where(parameter.Parameter)).SelectMany(_select);
	}

	public class WhereSelectSelector<TIn, TFrom, TTo> : ISelector<TFrom, TIn, TTo>
	{
		readonly Express<TIn, TFrom>          _where;
		readonly Expression<Func<TFrom, TTo>> _select;

		public WhereSelectSelector(Express<TIn, TFrom> @where, Expression<Func<TFrom, TTo>> select)
		{
			_where  = @where;
			_select = @select;
		}

		public IQueryable<TTo> Get(In<TFrom, TIn> parameter)
			=> parameter.Query.Where(_where(parameter.Parameter)).Select(_select);
	}

	public class WhereSelectionSelector<TIn, TFrom, TTo> : ISelector<TFrom, TIn, TTo>
	{
		readonly Express<TIn, TFrom>                      _where;
		readonly Func<IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public WhereSelectionSelector(Express<TIn, TFrom> @where, Func<IQueryable<TFrom>, IQueryable<TTo>> selection)
		{
			_where     = @where;
			_selection = selection;
		}

		public IQueryable<TTo> Get(In<TFrom, TIn> parameter)
			=> _selection(parameter.Query.Where(_where(parameter.Parameter)));
	}

	public class ParameterAwareSelectionSelector<TIn, TFrom, TTo> : ISelector<TFrom, TIn, TTo>
	{
		readonly Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public ParameterAwareSelectionSelector(Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			=> _selection = selection;

		public IQueryable<TTo> Get(In<TFrom, TIn> parameter) => _selection(parameter.Parameter, parameter.Query);
	}

	public class ParameterAwareWhereSelectionSelector<TIn, TFrom, TTo> : ISelector<TFrom, TIn, TTo>
	{
		readonly Express<TIn, TFrom>                           _where;
		readonly Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public ParameterAwareWhereSelectionSelector(Express<TIn, TFrom> where,
		                                            Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
		{
			_where     = @where;
			_selection = selection;
		}

		public IQueryable<TTo> Get(In<TFrom, TIn> parameter)
		{
			var query  = parameter.Query.Where(_where(parameter.Parameter));
			var result = _selection(parameter.Parameter, query);
			return result;
		}
	}
}