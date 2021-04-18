using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Selection
{
	public class Selector<TIn, TOut> : Instance<Expression<Func<TIn, TOut>>>
	{
		public static implicit operator Func<TIn, TOut>(Selector<TIn, TOut> value)
			=> Selectors<TIn, TOut>.Default.Get(value.Get());

		protected Selector(Expression<Func<TIn, TOut>> instance) : base(instance) {}
	}
}