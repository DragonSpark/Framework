using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Diagnostics;

public class RetryStrategy : ISelect<int, TimeSpan>
{
	readonly float _seconds;

	protected RetryStrategy(float seconds) => _seconds = seconds;

	public TimeSpan Get(int parameter)
	{
		var total = Math.Pow(_seconds, parameter);
		return TimeSpan.FromSeconds(total);
	}
}