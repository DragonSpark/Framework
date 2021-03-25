using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Diagnostics
{
	public sealed class RetryStrategy : Select<int, TimeSpan>
	{
		public static RetryStrategy Default { get; } = new RetryStrategy();

		RetryStrategy() : base(count => TimeSpan.FromSeconds(Math.Pow(2, count))) {}
	}

	public sealed class LinearRetryStrategy : ISelect<int, TimeSpan>
	{
		public static LinearRetryStrategy Default { get; } = new LinearRetryStrategy();

		LinearRetryStrategy() : this(TimeSpan.FromSeconds(1)) {}

		readonly TimeSpan _duration;

		public LinearRetryStrategy(TimeSpan duration) => _duration = duration;

		public TimeSpan Get(int parameter) => _duration * parameter;
	}
}