using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class WhereMany<TIn, T, TTo> : Combine<TIn, T, TTo>
{
	public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                 Expression<Func<T, bool>> where, Expression<Func<T, IEnumerable<TTo>>> select)
		: this(previous, (_, x) => where.Invoke(x), (_, x) => select.Invoke(x)) {}

	public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
	                 Expression<Func<TIn, T, IEnumerable<TTo>>> select)
		: this(previous, (_, x) => where.Invoke(x), select) {}

	public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where,
	                 Expression<Func<T, IEnumerable<TTo>>> select)
		: this(previous, where, (_, x) => select.Invoke(x)) {}

	public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where,
	                 Expression<Func<TIn, T, IEnumerable<TTo>>> select)
		: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x)).SelectMany(x => select.Invoke(@in, x))) {}

	public WhereMany(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where,
	                 Expression<Func<DbContext, TIn, T, IEnumerable<TTo>>> select)
		: base(previous,
		       (context, @in, q)
			       => q.Where(x => where.Invoke(@in, x)).SelectMany(x => select.Invoke(context, @in, x))) {}
}

public class WhereMany<T, TTo> : WhereMany<None, T, TTo>, IQuery<TTo>
{
	public WhereMany(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
	                 Expression<Func<T, IEnumerable<TTo>>> select)
		: base((context, _) => previous.Invoke(context), where, select) {}
}