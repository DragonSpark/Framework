using DragonSpark.Diagnostics;
using JetBrains.Annotations;

namespace DragonSpark.Application.Communication;

sealed class CircuitBreakerPolicy : CircuitBreakerPolicy<HttpResponseMessage>, ICommunicationsPolicy
{
	[UsedImplicitly]
	public static CircuitBreakerPolicy Default { get; } = new();

	CircuitBreakerPolicy() {}
}