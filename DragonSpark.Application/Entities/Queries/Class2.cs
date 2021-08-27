using DragonSpark.Model.Results;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	class Class2 {}

	public interface IQuery<TIn, T> : IResult<Expression<Func<DbContext, TIn, IQueryable<T>>>> {}

	public class InputQuery<TIn, T, TTo> : Instance<Expression<Func<DbContext, TIn, IQueryable<TTo>>>>, IQuery<TIn, TTo>
		where T : class
	{
		protected InputQuery(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			: this((DbContext context, TIn _) => select.Invoke(context.Set<T>())) {}

		protected InputQuery(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: this((DbContext context, TIn _) => select.Invoke(context, context.Set<T>())) {}

		protected InputQuery(Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> select)
			: this((context, @in) => select.Invoke(@in, context.Set<T>())) {}

		protected InputQuery(Expression<Func<TIn, DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: this((context, @in) => select.Invoke(@in, context, context.Set<T>())) {}

		protected InputQuery(Expression<Func<DbContext, TIn, IQueryable<TTo>>> instance) : base(instance) {}
	}

	public class InputQuery<TIn, T> : InputQuery<TIn, T, T> where T : class
	{
		protected InputQuery(Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<TIn, IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<TIn, DbContext, IQueryable<T>, IQueryable<T>>> select) : base(select) {}

		protected InputQuery(Expression<Func<DbContext, TIn, IQueryable<T>>> select) : base(select) {}
	}

	public class Combine<T> : Combine<T, T>
	{
		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(previous, select) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
			: base(previous, select) {}
	}

	public class Start<T> : Combine<T> where T : class
	{
		protected Start(Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(Set<T>.Default, select) {}

		protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
			: base(Set<T>.Default, select) {}
	}

	public class Start<T, TTo> : Combine<T, TTo> where T : class
	{
		protected Start(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select) : base(Set<T>.Default, select) {}

		protected Start(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: base(Set<T>.Default, select) {}
	}

	public class Combine<T, TTo> : Query<TTo>
	{
		public Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		               Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(previous.Invoke(context))) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(context, previous.Invoke(context))) {}
	}

	public class StartWhere<T> : Where<T> where T : class
	{
		protected StartWhere(Expression<Func<T, bool>> where) : base(Set<T>.Default, where) {}
	}

	public class Where<T> : Combine<T>
	{
		public Where(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
			: base(previous, x => x.Where(where)) {}
	}

	public class StartWhereSelect<T, TTo> : WhereSelect<T, TTo> where T : class
	{
		protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: base(Set<T>.Default, where, select) {}
	}

	public class WhereSelect<T, TTo> : Combine<T, TTo>
	{
		protected WhereSelect(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                      Expression<Func<T, TTo>> select)
			: base(previous, x => x.Where(where).Select(select)) {}
	}

	public class StartSelect<TFrom, TTo> : Select<TFrom, TTo> where TFrom : class
	{
		protected StartSelect(Expression<Func<TFrom, TTo>> select) : base(Set<TFrom>.Default, select) {}
	}

	public class Select<TFrom, TTo> : Combine<TFrom, TTo>
	{
		public Select(Expression<Func<DbContext, IQueryable<TFrom>>> previous, Expression<Func<TFrom, TTo>> select)
			: base(previous, x => x.Select(select)) {}
	}

	public class StartSelectMany<TFrom, TTo> : SelectMany<TFrom, TTo> where TFrom : class
	{
		protected StartSelectMany(Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(Set<TFrom>.Default, select) {}
	}

	public class SelectMany<TFrom, TTo> : Combine<TFrom, TTo>
	{
		public SelectMany(Expression<Func<DbContext, IQueryable<TFrom>>> previous,
		                  Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(previous, x => x.SelectMany(select)) {}
	}

	public class StartIntroduce<TFrom, TOther, TTo> : Introduce<TFrom, TOther, TTo> where TFrom : class
	{
		public StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
		                      Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, other, select) {}

		public StartIntroduce(Expression<Func<DbContext, IQueryable<TOther>>> other,
		                      Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>>
			                      select)
			: base(Set<TFrom>.Default, other, select) {}
	}

	public class StartIntroduce<TFrom, T1, T2, TTo> : Introduce<TFrom, T1, T2, TTo> where TFrom : class
	{
		public StartIntroduce(
			(Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>) others,
			Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}

		public StartIntroduce(
			(Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>) others,
			Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}
	}

	public class StartIntroduce<TFrom, T1, T2, T3, TTo> : Introduce<TFrom, T1, T2, T3, TTo> where TFrom : class
	{
		public StartIntroduce((Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                      Expression<Func<DbContext, IQueryable<T3>>>) others,
		                      Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                      IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}

		public StartIntroduce((Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                      Expression<Func<DbContext, IQueryable<T3>>>) others,
		                      Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
			                      IQueryable<T3>, IQueryable<TTo>>> select)
			: base(Set<TFrom>.Default, others, select) {}
	}

	public class Introduce<TFrom, TOther, TTo> : Query<TTo>
	{
		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(context, from.Invoke(context), other.Invoke(context))) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 Expression<Func<DbContext, IQueryable<TOther>>> other,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<TOther>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(from.Invoke(context), other.Invoke(context))) {}
	}

	public class Introduce<TFrom, T1, T2, TTo> : Query<TTo>
	{
		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>)
			                 others,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>>
			                 select)
			: base(context => select.Invoke(context, from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context))) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>)
			                 others,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
			: base(context => select.Invoke(from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context))) {}
	}

	public class Introduce<TFrom, T1, T2, T3, TTo> : Query<TTo>
	{
		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                 Expression<Func<DbContext, IQueryable<T3>>>) others,
		                 Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                 IQueryable<TTo>>> select)
			: base(context => select.Invoke(context, from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context), others.Item3.Invoke(context))) {}

		public Introduce(Expression<Func<DbContext, IQueryable<TFrom>>> from,
		                 (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			                 Expression<Func<DbContext, IQueryable<T3>>>) others,
		                 Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			                 IQueryable<TTo>>> select)
			: base(context => select.Invoke(from.Invoke(context), others.Item1.Invoke(context),
			                                others.Item2.Invoke(context), others.Item3.Invoke(context))) {}
	}

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