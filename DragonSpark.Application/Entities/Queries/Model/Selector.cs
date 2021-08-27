using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Model
{
	public class Selector<TIn, TOut> : Instance<Expression<Func<TIn, TOut>>>
	{
		protected Selector(Expression<Func<TIn, TOut>> instance) : base(instance) {}
	}
}