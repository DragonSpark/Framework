namespace DragonSpark.Diagnostics;

public sealed class DefaultRetryPolicy<T> : RetryPolicy<T>
{
	public static DefaultRetryPolicy<T> Default { get; } = new();

	DefaultRetryPolicy() {}
}

public sealed class DefaultRetryPolicyBuilder : RetryPolicyBuilder
{
	public static DefaultRetryPolicyBuilder Default { get; } = new();

	DefaultRetryPolicyBuilder() {}
}