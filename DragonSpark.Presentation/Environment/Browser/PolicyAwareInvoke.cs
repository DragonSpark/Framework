using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Presentation.Environment.Browser;

public class PolicyAwareInvoke<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected PolicyAwareInvoke(ISelecting<TIn, TOut> previous) : base(previous, DurableEvaluatePolicy.Default.Get()) {}
}