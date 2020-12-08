using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Hosting.xUnit
{
	public sealed class Epoch : Instance<DateTimeOffset>, ITime
	{
		public static Epoch Default { get; } = new Epoch();

		Epoch() : base(new DateTimeOffset(1976, 6, 7, 23, 17, 24,
		                                  TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
		                                              .BaseUtcOffset)) {}
	}
}