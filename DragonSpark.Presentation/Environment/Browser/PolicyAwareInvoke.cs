using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Environment.Browser;

public class PolicyAwareInvoke<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	public PolicyAwareInvoke(ISelecting<TIn, TOut> previous) : base(previous, DurableEvaluatePolicy.Default.Get()) {}
}