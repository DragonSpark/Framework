using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartWhereSelect<T, TTo> : WhereSelect<T, TTo> where T : class
{
	protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: base(Set<T>.Default.Then(), where, select) {}
}

public class StartWhereSelect<TIn, T, TTo> : WhereSelect<TIn, T, TTo> where T : class
{
	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: base((context, @in) => query.Invoke(Set<TIn, T>.Default.Get().Invoke(context, @in)), where, select) {}

	protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<TIn, T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<TIn, T, bool>> where, Expression<Func<DbContext, T, TTo>> select)
		: base((context, @in) => query.Invoke(Set<TIn, T>.Default.Get().Invoke(context, @in)), where,
		       (context, _, x) => select.Invoke(context, x)) {}

	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<TIn, T, bool>> where, Expression<Func<T, TTo>> select)
		: base((context, @in) => query.Invoke(Set<TIn, T>.Default.Get().Invoke(context, @in)), where, select) {}

	protected StartWhereSelect(Expression<Func<TIn, T, bool>> where, Expression<Func<T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereSelect(Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereSelect(Expression<Func<TIn, T, bool>> where,
	                           Expression<Func<DbContext, TIn, T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}
}