using DragonSpark.Diagnostics;
using System;

namespace DragonSpark.Application.Entities.Diagnostics
{
	public sealed class ApplicationContentRetryPolicy : RetryPolicy
	{
		public static ApplicationContentRetryPolicy Default { get; } = new ApplicationContentRetryPolicy();

		ApplicationContentRetryPolicy() : base(10, new LinearRetryStrategy(TimeSpan.FromMilliseconds(50)).Get) {}
	}
}