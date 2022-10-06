using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Application.Connections.Client;

public sealed class DurableEvaluatePolicy : Deferred<IAsyncPolicy>
{
	public static DurableEvaluatePolicy Default { get; } = new();

	DurableEvaluatePolicy() : base(SubscriptionBuilder.Default.Then().Select(DefaultRetryPolicy.Default)) {}
}