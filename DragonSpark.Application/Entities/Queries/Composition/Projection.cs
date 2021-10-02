using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class Projection<TFrom, TTo> : Instance<Expression<Func<TFrom, TTo>>>, IProjection<TFrom, TTo>
	{
		protected Projection(Expression<Func<TFrom, TTo>> instance) : base(instance) {}
	}

}