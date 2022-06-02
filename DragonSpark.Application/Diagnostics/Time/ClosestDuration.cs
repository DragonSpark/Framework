using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Diagnostics.Time;

public sealed class ClosestDuration : ISelect<ClosestDurationInput, TimeSpan>
{
	public static ClosestDuration Default { get; } = new();

	ClosestDuration() : this(DragonSpark.Runtime.Time.Default) {}

	readonly ITime _time;

	public ClosestDuration(ITime time) => _time = time;

	public TimeSpan Get(ClosestDurationInput parameter)
	{
		var (until, interval) = parameter;

		var time    = _time.Get();
		var current = time + interval;
		var result  = current > until ? until - time : interval;
		return result;
	}
}