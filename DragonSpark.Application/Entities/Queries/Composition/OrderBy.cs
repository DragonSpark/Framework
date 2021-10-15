using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class OrderBy<T, TProperty> : OrderBy<None, T, TProperty>, IQuery<T>
{
	public OrderBy(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, TProperty>> property)
		: base(previous.Then(), property) {}
}

public class OrderBy<TIn, T, TProperty> : Combine<TIn, T, T>
{
	public OrderBy(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	               Expression<Func<T, TProperty>> property)
		: this(previous, (_, element) => property.Invoke(element)) {}

	public OrderBy(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	               Expression<Func<TIn, T, TProperty>> property)
		: base(previous, (@in, q) => q.OrderBy(x => property.Invoke(@in, x))) {}
}