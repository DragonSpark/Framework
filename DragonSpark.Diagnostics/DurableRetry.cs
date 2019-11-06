using Polly;
using Serilog;
using DragonSpark.Runtime.Invocation.Operations;

namespace DragonSpark.Diagnostics
{
	public class DurableRetry : DurableObservableSource<object>
	{
		public DurableRetry(ILogger logger, PolicyBuilder policy)
			: base(new RetryPolicies(new LogRetryException(logger).Execute).Get(policy)) {}
	}

	public class DurableRetry<T> : DurableObservableSource<T>
	{
		public DurableRetry(ILogger logger, PolicyBuilder<T> policy)
			: base(new RetryPolicies<T>(new LogRetryException<T>(logger).Execute).Get(policy)) {}
	}
}