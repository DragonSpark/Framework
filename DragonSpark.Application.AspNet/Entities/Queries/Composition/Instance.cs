using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class Instance<T> : Instance<None, T>, IInstance<T>
{
	protected Instance(Expression<Func<DbContext, T>> instance) : base(instance) {}

	protected Instance(Expression<Func<DbContext, None, T>> instance) : this(instance.Then().Elide().Get()) {}
}

public class Instance<TIn, T> : DragonSpark.Model.Results.Instance<Expression<Func<DbContext, TIn, T>>>,
								IInstance<TIn, T>
{
	protected Instance(Expression<Func<DbContext, T>> instance) : base((context, _) => instance.Invoke(context)) {}

	protected Instance(Expression<Func<TIn, T>> instance) : base((_, @in) => instance.Invoke(@in)) {}

	protected Instance(Expression<Func<DbContext, TIn, T>> instance) : base(instance) {}
}