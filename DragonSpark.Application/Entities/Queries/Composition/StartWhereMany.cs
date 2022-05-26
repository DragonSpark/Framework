using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartWhereMany<TIn, T, TTo> : WhereMany<TIn, T, TTo> where T : class
{
	protected StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<TIn, T, IEnumerable<TTo>>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereMany(Expression<Func<IQueryable<T>, IQueryable<T>>> start,
	                         Expression<Func<TIn, T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
		: base((context, _) => start.Invoke(context.Set<T>()), where, select) {}

	protected StartWhereMany(Expression<Func<TIn, T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereMany(Expression<Func<IQueryable<T>, IQueryable<T>>> start,
	                         Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, IEnumerable<TTo>>> select)
		: base((context, _) => start.Invoke(context.Set<T>()), where, select) {}

	protected StartWhereMany(Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, IEnumerable<TTo>>> select)
		: base(Set<TIn, T>.Default, where, select) {}

	protected StartWhereMany(Expression<Func<TIn, T, bool>> where,
	                         Expression<Func<DbContext, TIn, T, IEnumerable<TTo>>> select)
		: base(Set<TIn, T>.Default, where, select) {}
}

public class StartWhereMany<T, TTo> : WhereMany<T, TTo> where T : class
{
	protected StartWhereMany(Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
		: this(x => x, where, select) {}

	protected StartWhereMany(Expression<Func<IQueryable<T>, IQueryable<T>>> previous,
	                         Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
		: base(context => previous.Invoke(context.Set<T>()), where, select) {}
}