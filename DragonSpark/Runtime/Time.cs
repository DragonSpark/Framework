using System;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime
{
	public sealed class Time : Result<DateTimeOffset>, ITime
	{
		public static Time Default { get; } = new Time();

		Time() : base(() => DateTimeOffset.Now) {}
	}
}