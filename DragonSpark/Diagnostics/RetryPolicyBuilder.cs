using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics;

public class RetryPolicy<T> : IPolicy<T>
{
	readonly byte                _times;
	readonly Func<int, TimeSpan> _strategy;

	protected RetryPolicy() : this(5, DefaultJitterStrategy.Default.Get) {}

	protected RetryPolicy(byte times, Func<int, TimeSpan> strategy)
	{
		_times    = times;
		_strategy = strategy;
	}

	public IAsyncPolicy<T> Get(PolicyBuilder<T> parameter) => parameter.WaitAndRetryAsync(_times, _strategy);
}

public class RetryPolicyBuilder : IPolicy
{
	readonly byte                            _times;
	readonly Func<int, TimeSpan>             _strategy;
	readonly Func<Exception, TimeSpan, Task> _retry;

	protected RetryPolicyBuilder(byte times = 5) : this(times, DefaultJitterStrategy.Default.Get) {}

	protected RetryPolicyBuilder(byte times, Func<int, TimeSpan> strategy)
		: this(times, strategy, (_, _) => Task.CompletedTask) {}

	protected RetryPolicyBuilder(Func<Exception, TimeSpan, Task> retry) : this(5, retry) {}

	protected RetryPolicyBuilder(byte times, Func<Exception, TimeSpan, Task> retry)
		: this(times, DefaultJitterStrategy.Default.Get, retry) {}

	protected RetryPolicyBuilder(byte times, Func<int, TimeSpan> strategy, Func<Exception, TimeSpan, Task> retry)
	{
		_times    = times;
		_strategy = strategy;
		_retry    = retry;
	}

	public IAsyncPolicy Get(PolicyBuilder parameter) => parameter.WaitAndRetryAsync(_times, _strategy, _retry);
}