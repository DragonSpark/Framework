using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class Where<T> : Where<None, T>, IQuery<T>
{
	public Where(Expression<Func<DbContext, None, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
		: base(previous, where) {}

	public Where(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
		: base(previous.Then().Accept(), where) {}
}

public class Where<TIn, T> : Combine<TIn, T, T>
{
	public Where(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
		: this((context, _) => previous.Invoke(context), where) {}

	protected Where(IQuery<T> previous, Expression<Func<TIn, T, bool>> where) : this(previous.Then(), where) {}

	protected Where(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where)
		: this((context, _) => previous.Invoke(context), where) {}

	public Where(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
		: this(previous, (_, element) => where.Invoke(element)) {}

	public Where(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where)
		: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x))) {}
}