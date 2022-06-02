using System;

namespace DragonSpark.Application.Diagnostics.Time;

public readonly record struct ClosestDurationInput(DateTimeOffset Until, TimeSpan Interval);