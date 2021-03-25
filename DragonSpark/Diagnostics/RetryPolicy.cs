﻿using Polly;
using System;

namespace DragonSpark.Diagnostics
{
	public class RetryPolicy<T> : IPolicy<T>
	{
		readonly byte                _times;
		readonly Func<int, TimeSpan> _strategy;

		protected RetryPolicy() : this(5, JitterStrategy.Default.Get) {}

		public RetryPolicy(byte times, Func<int, TimeSpan> strategy)
		{
			_times    = times;
			_strategy = strategy;
		}

		public IAsyncPolicy<T> Get(PolicyBuilder<T> parameter) => parameter.WaitAndRetryAsync(_times, _strategy);
	}

	public class RetryPolicy : IPolicy
	{
		readonly byte                _times;
		readonly Func<int, TimeSpan> _strategy;

		protected RetryPolicy() : this(5, JitterStrategy.Default.Get) {}

		public RetryPolicy(byte times, Func<int, TimeSpan> strategy)
		{
			_times    = times;
			_strategy = strategy;
		}

		public IAsyncPolicy Get(PolicyBuilder parameter) => parameter.WaitAndRetryAsync(_times, _strategy);
	}
}