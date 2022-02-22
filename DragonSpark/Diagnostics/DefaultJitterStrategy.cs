namespace DragonSpark.Diagnostics;

sealed class DefaultJitterStrategy : JitterStrategy
{
	public static DefaultJitterStrategy Default { get; } = new();

	DefaultJitterStrategy() : base(DefaultRetryStrategy.Default) {}
}