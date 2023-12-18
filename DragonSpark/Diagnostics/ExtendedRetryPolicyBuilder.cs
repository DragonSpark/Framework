namespace DragonSpark.Diagnostics;

public sealed class ExtendedRetryPolicyBuilder : RetryPolicyBuilder
{
	public static ExtendedRetryPolicyBuilder Default { get; } = new();

	ExtendedRetryPolicyBuilder() : base(15) {}
}