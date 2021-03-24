using Polly;
using System;

namespace DragonSpark.Diagnostics
{
	public class CircuitBreakerPolicy<T> : IPolicy<T>
	{
		readonly byte     _attempts;
		readonly TimeSpan _break;

		protected CircuitBreakerPolicy() : this(5, TimeSpan.FromSeconds(30)) {}

		public CircuitBreakerPolicy(byte attempts, TimeSpan @break)
		{
			_attempts = attempts;
			_break    = @break;
		}

		public IAsyncPolicy<T> Get(PolicyBuilder<T> parameter) => parameter.CircuitBreakerAsync(_attempts, _break);
	}

	public class CircuitBreakerPolicy : IPolicy
	{
		readonly byte     _attempts;
		readonly TimeSpan _break;

		protected CircuitBreakerPolicy() : this(5, TimeSpan.FromSeconds(30)) {}

		public CircuitBreakerPolicy(byte attempts, TimeSpan @break)
		{
			_attempts = attempts;
			_break    = @break;
		}

		public IAsyncPolicy Get(PolicyBuilder parameter) => parameter.CircuitBreakerAsync(_attempts, _break);
	}

}