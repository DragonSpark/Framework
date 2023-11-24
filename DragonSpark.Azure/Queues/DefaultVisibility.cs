using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Azure.Queues;

sealed class DefaultVisibility : Instance<TimeSpan>
{
	public static DefaultVisibility Default { get; } = new();

	DefaultVisibility() : base(TimeSpan.Zero) {}
}