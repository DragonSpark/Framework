using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Selection.Conditions
{
	public class Conditional<TIn, TOut> : Validated<TIn, TOut>, IConditional<TIn, TOut>
	{
		readonly static Func<TIn, TOut> Fallback = Start.A.Selection<TIn>().By.Default<TOut>().Get().Get;

		public Conditional(ICondition<TIn> condition, IConditional<TIn, TOut> source)
			: this(source.Condition.Then().Or(condition), source.Get) {}

		public Conditional(Func<TIn, bool> condition, Func<TIn, TOut> source) : this(condition, source, Fallback) {}

		public Conditional(Func<TIn, bool> condition, Func<TIn, TOut> source, Func<TIn, TOut> fallback)
			: this(condition.Target as ICondition<TIn> ?? new Condition<TIn>(condition), source, fallback) {}

		public Conditional(ICondition<TIn> condition, ISelect<TIn, TOut> select) : this(condition, select.Get) {}

		public Conditional(ICondition<TIn> condition, Func<TIn, TOut> source) : this(condition, source, Fallback) {}

		public Conditional(ICondition<TIn> condition, Func<TIn, TOut> source, Func<TIn, TOut> fallback)
			: base(condition.Get, source, fallback) => Condition = condition;

		public ICondition<TIn> Condition { get; }
	}
}