using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;
using System;
using System.Net.Http;
using Policy = Polly.Policy;

namespace DragonSpark.Application.Connections.Events;

public class DurableConnectionPolicyBase : DurableConnectionPolicyBase<HttpRequestException>
{
	protected DurableConnectionPolicyBase(RetryPolicy builder) : base(builder) {}
}

public class DurableConnectionPolicyBase<T> : Deferred<IAsyncPolicy> where T : Exception
{
	protected DurableConnectionPolicyBase(RetryPolicy builder)
		: base(Start.A.Result(Policy.Handle<T>()).Select(builder)) {}
}