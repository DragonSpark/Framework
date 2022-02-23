using DragonSpark.Diagnostics;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class PolicyAwareEvaluate : PolicyAwareOperation<string>, IEvaluate
{
	public PolicyAwareEvaluate(IEvaluate previous) : base(previous, DurableEvaluatePolicy.Default.Get()) {}
}