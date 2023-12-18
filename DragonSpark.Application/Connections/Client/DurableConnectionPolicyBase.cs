using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Application.Connections.Client;

public class DurableConnectionPolicyBase : Deferred<IAsyncPolicy>
{
	protected DurableConnectionPolicyBase(RetryPolicyBuilder builder)
		: base(SubscriptionBuilder.Default.Then().Select(builder)) {}
}