using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Diagnostics;

public sealed class LinearRetryStrategy : ISelect<int, TimeSpan>
{
	public static LinearRetryStrategy Default { get; } = new LinearRetryStrategy();

	LinearRetryStrategy() : this(TimeSpan.FromSeconds(1)) {}

	readonly TimeSpan _duration;

	public LinearRetryStrategy(TimeSpan duration) => _duration = duration;

	public TimeSpan Get(int parameter) => _duration * parameter;
}