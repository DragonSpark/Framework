using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartWhereSelect<T, TTo> : WhereSelect<T, TTo> where T : class
{
	protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: this(q => q, where, select) {}

	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: base(context => query.Invoke(Set<T>.Default.Get().Invoke(context, None.Default)), where, select) {}

	protected StartWhereSelect(Expression<Func<T, bool>> where, Expression<Func<DbContext, T, TTo>> select)
		: this(q => q, where, select) {}

	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<T, bool>> where, Expression<Func<DbContext, T, TTo>> select)
		: base(d => query.Invoke(Set<T>.Default.Get().Invoke(d, None.Default)), where,
			(d, x) => select.Invoke(d, x)) {}
}

public class StartWhereSelect<TIn, T, TTo> : WhereSelect<TIn, T, TTo> where T : class
{
	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: base((d, @in) => query.Invoke(Set<TIn, T>.Default.Get().Invoke(d, @in)), where, select) {}

	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<DbContext, TIn, T, bool>> where, Expression<Func<T, TTo>> select)
		: base(query, where, select) {}

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

	protected StartWhereSelect(Expression<Func<DbContext, TIn, T, bool>> where, Expression<Func<T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereSelect(Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereSelect(Expression<Func<TIn, T, bool>> where,
	                           Expression<Func<DbContext, TIn, T, TTo>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<DbContext, TIn, T, bool>> where,
	                           Expression<Func<DbContext, TIn, T, TTo>> select)
		: base((context, @in) => query.Invoke(Set<TIn, T>.Default.Get().Invoke(context, @in)), where, select) {}

	protected StartWhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> query,
	                           Expression<Func<TIn, T, bool>> where,
	                           Expression<Func<DbContext, TIn, T, TTo>> select)
		: base((context, @in) => query.Invoke(Set<TIn, T>.Default.Get().Invoke(context, @in)), where, select) {}
}