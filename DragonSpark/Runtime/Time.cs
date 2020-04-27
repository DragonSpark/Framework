using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Runtime
{
	public sealed class Time : Result<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.UtcNow) {}
	}
}