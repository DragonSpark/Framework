using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class DurableEvaluatePolicy : DeferredSingleton<IAsyncPolicy>
{
	public static DurableEvaluatePolicy Default { get; } = new();

	DurableEvaluatePolicy() : base(EvaluatePolicy.Default.Then().Select(DefaultRetryPolicy.Default)) {}
}