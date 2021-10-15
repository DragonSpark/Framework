using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class SelectMany<TIn, TFrom, TTo> : Combine<TIn, TFrom, TTo>
{
	public SelectMany(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
	                  Expression<Func<TFrom, IEnumerable<TTo>>> select)
		: this(previous, (_, x) => select.Invoke(x)) {}

	public SelectMany(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
	                  Expression<Func<TIn, TFrom, IEnumerable<TTo>>> select)
		: base(previous, (@in, q) => q.SelectMany(x => select.Invoke(@in, x))) {}

	protected SelectMany(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
	                     Expression<Func<DbContext, TIn, TFrom, IEnumerable<TTo>>> select)
		: base(previous, (context, @in, q) => q.SelectMany(x => select.Invoke(context, @in, x))) {}
}

public class SelectMany<TFrom, TTo> : SelectMany<None, TFrom, TTo>, IQuery<TTo>
{
	public SelectMany(Expression<Func<DbContext, IQueryable<TFrom>>> previous,
	                  Expression<Func<TFrom, IEnumerable<TTo>>> select)
		: base((context, _) => previous.Invoke(context), select) {}
}