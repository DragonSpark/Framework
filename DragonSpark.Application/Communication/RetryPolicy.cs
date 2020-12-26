using Polly;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Communication
{
	sealed class RetryPolicy : ICommunicationsPolicy
	{
		public static RetryPolicy Default { get; } = new RetryPolicy();

		RetryPolicy() : this(5, JitterStrategy.Default.Get) {}

		readonly byte                _times;
		readonly Func<int, TimeSpan> _strategy;

		public RetryPolicy(byte times, Func<int, TimeSpan> strategy)
		{
			_times    = times;
			_strategy = strategy;
		}

		public AsyncPolicy<HttpResponseMessage> Get(PolicyBuilder<HttpResponseMessage> parameter)
			=> parameter.WaitAndRetryAsync(_times, _strategy);
	}
}