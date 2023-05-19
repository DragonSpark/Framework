using DragonSpark.Model.Results;
using System;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class DefaultRequestMemoryTimeSpan : Instance<TimeSpan>
{
	public static DefaultRequestMemoryTimeSpan Default { get; } = new();

	DefaultRequestMemoryTimeSpan() : base(TimeSpan.FromSeconds(3)) {}
}