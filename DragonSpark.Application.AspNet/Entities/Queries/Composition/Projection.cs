using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public class Projection<TFrom, TTo> : DragonSpark.Model.Results.Instance<Expression<Func<TFrom, TTo>>>,
									  IProjection<TFrom, TTo>
{
	public static implicit operator Func<TFrom, TTo>(Projection<TFrom, TTo> instance) => instance.Get().Compile();

	protected Projection(Expression<Func<TFrom, TTo>> instance) : base(instance) {}
}

public class Projection<TIn, TFrom, TTo> : DragonSpark.Model.Results.Instance<Expression<Func<TIn, TFrom, TTo>>>
{
	public static implicit operator Func<TIn, TFrom, TTo>(Projection<TIn, TFrom, TTo> instance)
		=> instance.Get().Compile();

	protected Projection(Expression<Func<TIn, TFrom, TTo>> instance) : base(instance) {}
}