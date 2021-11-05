using DragonSpark.Model;
using DragonSpark.Model.Results;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

internal class Class1 {}

public interface IInstance<T> : IInstance<None, T> {}

public interface IInstance<TIn, T> : IResult<Expression<Func<DbContext, TIn, T>>> {}

public class Instance<T> : Instance<None, T>, IInstance<T>
{
	public Instance(Expression<Func<DbContext, T>> instance) : base(instance) {}

	public Instance(Expression<Func<DbContext, None, T>> instance) : this(instance.Then().Elide().Get()) {}
}

public class Instance<TIn, T> : DragonSpark.Model.Results.Instance<Expression<Func<DbContext, TIn, T>>>,
								IInstance<TIn, T>
{
	public Instance(Expression<Func<DbContext, T>> instance)
		: base((context, _) => instance.Invoke(context)) {}

	public Instance(Expression<Func<TIn, T>> instance)
		: base((_, @in) => instance.Invoke(@in)) {}

	public Instance(Expression<Func<DbContext, TIn, T>> instance) : base(instance) {}
}