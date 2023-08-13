using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class ContextProjection<TFrom, TTo>
	: DragonSpark.Model.Results.Instance<Expression<Func<DbContext, TFrom, TTo>>>, IContextProjection<TFrom, TTo>
{
	protected ContextProjection(Expression<Func<TFrom, TTo>> instance) : this((_, x) => instance.Invoke(x)) {}

	protected ContextProjection(Expression<Func<DbContext, TFrom, TTo>> instance) : base(instance) {}
}

public class ContextProjection<TIn, TFrom, TTo>
	: DragonSpark.Model.Results.Instance<Expression<Func<DbContext, TIn, TFrom, TTo>>>
{
	protected ContextProjection(Expression<Func<TFrom, TTo>> instance) : this((_, _, x) => instance.Invoke(x)) {}

	protected ContextProjection(Expression<Func<DbContext, TIn, TFrom, TTo>> instance) : base(instance) {}
}