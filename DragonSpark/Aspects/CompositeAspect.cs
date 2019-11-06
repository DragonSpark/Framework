using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Aspects
{
	public class CompositeAspect<TIn, TOut> : Aggregate<IAspect<TIn, TOut>, ISelect<TIn, TOut>>, IAspect<TIn, TOut>
	{
		public CompositeAspect(params IAspect<TIn, TOut>[] aspects) : this(new Array<IAspect<TIn, TOut>>(aspects)) {}

		public CompositeAspect(Array<IAspect<TIn, TOut>> items) : base(items) {}
	}
}