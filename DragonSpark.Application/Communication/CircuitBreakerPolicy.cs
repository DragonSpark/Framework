using Polly;
using System;
using System.Net.Http;

namespace DragonSpark.Application.Communication
{
	sealed class CircuitBreakerPolicy : ICommunicationsPolicy
	{
		public static CircuitBreakerPolicy Default { get; } = new CircuitBreakerPolicy();

		CircuitBreakerPolicy() : this(5, TimeSpan.FromSeconds(30)) {}

		readonly byte     _attempts;
		readonly TimeSpan _break;

		public CircuitBreakerPolicy(byte attempts, TimeSpan @break)
		{
			_attempts = attempts;
			_break    = @break;
		}

		public AsyncPolicy<HttpResponseMessage> Get(PolicyBuilder<HttpResponseMessage> parameter)
			=> parameter.CircuitBreakerAsync(_attempts, _break);
	}
}