using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Application.Connections.Client;

public sealed class DurableConnectionPolicy : Deferred<IAsyncPolicy>
{
	public static DurableConnectionPolicy Default { get; } = new();

	DurableConnectionPolicy() : base(SubscriptionBuilder.Default.Then().Select(DefaultRetryPolicyBuilder.Default)) {}
}