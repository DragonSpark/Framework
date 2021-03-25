using DragonSpark.Diagnostics;
using System.Net.Http;

namespace DragonSpark.Application.Communication
{
	sealed class CircuitBreakerPolicy : CircuitBreakerPolicy<HttpResponseMessage>, ICommunicationsPolicy
	{
		public static CircuitBreakerPolicy Default { get; } = new CircuitBreakerPolicy();

		CircuitBreakerPolicy() {}
	}
}