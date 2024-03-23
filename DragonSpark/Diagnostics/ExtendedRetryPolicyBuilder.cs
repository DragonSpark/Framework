using JetBrains.Annotations;

namespace DragonSpark.Diagnostics;

public sealed class ExtendedRetryPolicyBuilder : RetryPolicy
{
	[UsedImplicitly]
	public static ExtendedRetryPolicyBuilder Default { get; } = new();

	ExtendedRetryPolicyBuilder() : base(15) {}
}