namespace DragonSpark.Diagnostics
{
	public sealed class DefaultCircuitBreakerPolicy<T> : CircuitBreakerPolicy<T>
	{
		public static DefaultCircuitBreakerPolicy<T> Default { get; } = new();

		DefaultCircuitBreakerPolicy() {}
	}

	public sealed class DefaultCircuitBreakerPolicy : CircuitBreakerPolicy
	{
		public static DefaultCircuitBreakerPolicy Default { get; } = new();

		DefaultCircuitBreakerPolicy() {}
	}
}