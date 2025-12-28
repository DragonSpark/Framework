using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Presentation.Environment.Browser;

public class PolicyAwareInvoke<TIn, TOut> : PolicyAware<TIn, TOut>
{
	protected PolicyAwareInvoke(IStopAware<TIn, TOut> previous) : base(previous, DurableEvaluatePolicy.Default) {}
}