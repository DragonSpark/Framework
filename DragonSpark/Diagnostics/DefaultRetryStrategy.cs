namespace DragonSpark.Diagnostics;

public sealed class DefaultRetryStrategy : RetryStrategy
{
	public static DefaultRetryStrategy Default { get; } = new();

	DefaultRetryStrategy() : base(1.6f) {}
}