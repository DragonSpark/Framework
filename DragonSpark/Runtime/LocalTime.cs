using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Runtime;

public sealed class LocalTime : Result<DateTimeOffset>, ITime
{
	public static LocalTime Default { get; } = new LocalTime();

	LocalTime() : base(() => DateTimeOffset.Now) {}
}