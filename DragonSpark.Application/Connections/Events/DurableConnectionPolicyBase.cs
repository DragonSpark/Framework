using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Connections.Events;

public class DurableConnectionPolicyBase : Deferred<IAsyncPolicy>
{
	protected DurableConnectionPolicyBase(RetryPolicy policy)
		: this(Policy.Handle<HttpRequestException>(), policy) {}

	protected DurableConnectionPolicyBase(PolicyBuilder builder) : this(builder, DefaultRetryPolicy.Default) {}

	protected DurableConnectionPolicyBase(PolicyBuilder builder, RetryPolicy policy)
		: base(Start.A.Result(builder).Select(policy)) {}
}

public class DurableConnectionPolicyBase<T> : DurableConnectionPolicyBase where T : Exception
{
	protected DurableConnectionPolicyBase(RetryPolicy builder)
		: base(Policy.Handle<T>(), builder) {}
}