using System;
using Polly;
using Polly.Retry;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics
{
	public sealed class RetryPolicies : ISelect<PolicyBuilder, ISyncPolicy>
	{
		readonly Action<Exception, TimeSpan> _onRetry;
		readonly int                         _retries;
		readonly Func<int, TimeSpan>         _time;

		public RetryPolicies(Action<Exception, TimeSpan> onRetry, int retries = 10)
			: this(LinearRetryTime.Default.Get, onRetry, retries) {}

		public RetryPolicies(Func<int, TimeSpan> time, Action<Exception, TimeSpan> onRetry, int retries)
		{
			_retries = retries;
			_time    = time;
			_onRetry = onRetry;
		}

		public ISyncPolicy Get(PolicyBuilder parameter) => parameter.WaitAndRetry(_retries, _time, _onRetry);
	}

	public sealed class RetryPolicies<T> : ISelect<PolicyBuilder<T>, RetryPolicy<T>>
	{
		readonly Action<DelegateResult<T>, TimeSpan, Context> _onRetry;
		readonly int                                          _retries;
		readonly Func<int, TimeSpan>                          _time;

		public RetryPolicies(Action<DelegateResult<T>, TimeSpan, Context> onRetry, int retries = 10)
			: this(LinearRetryTime.Default.Get, onRetry, retries) {}

		public RetryPolicies(Func<int, TimeSpan> time, Action<DelegateResult<T>, TimeSpan, Context> onRetry,
		                     int retries)
		{
			_retries = retries;
			_time    = time;
			_onRetry = onRetry;
		}

		public RetryPolicy<T> Get(PolicyBuilder<T> parameter) => parameter.WaitAndRetry(_retries, _time, _onRetry);
	}
}