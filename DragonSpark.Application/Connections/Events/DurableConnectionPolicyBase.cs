using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Application.Connections.Events;

public class DurableConnectionPolicyBase : Deferred<IAsyncPolicy>
{
	protected DurableConnectionPolicyBase(RetryPolicy builder) : base(RequestBuilder.Default.Then().Select(builder)) {}
}