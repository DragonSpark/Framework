using DragonSpark.Diagnostics;
using System;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class ApplicationContentRetryPolicyBuilder : RetryPolicy
{
	public static ApplicationContentRetryPolicyBuilder Default { get; } = new();

	ApplicationContentRetryPolicyBuilder() : base(15, new LinearRetryStrategy(TimeSpan.FromMilliseconds(100)).Get) {}
}