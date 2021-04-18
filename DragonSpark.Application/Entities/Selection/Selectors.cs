using DragonSpark.Model.Selection.Stores;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Selection
{
	public sealed class Selectors<TIn, TOut> : ReferenceValueStore<Expression<Func<TIn, TOut>>, Func<TIn, TOut>>
	{
		public static Selectors<TIn,TOut> Default { get; } = new Selectors<TIn,TOut>();

		Selectors() : base(x => x.Compile()) {}
	}
}