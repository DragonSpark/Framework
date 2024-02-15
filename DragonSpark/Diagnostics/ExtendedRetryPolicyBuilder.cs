﻿namespace DragonSpark.Diagnostics;

public sealed class ExtendedRetryPolicyBuilder : RetryPolicy
{
	public static ExtendedRetryPolicyBuilder Default { get; } = new();

	ExtendedRetryPolicyBuilder() : base(15) {}
}