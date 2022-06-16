using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class DurableEvaluatePolicy : Deferred<IAsyncPolicy>
{
	public static DurableEvaluatePolicy Default { get; } = new();

	DurableEvaluatePolicy() : base(EvaluateBuilder.Default.Then().Select(DefaultRetryPolicy.Default)) {}
}