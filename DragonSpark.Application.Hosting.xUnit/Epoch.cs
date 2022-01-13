using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Hosting.xUnit;

public sealed class Epoch : Instance<DateTimeOffset>, ITime
{
	public static Epoch Default { get; } = new Epoch();

	Epoch() : base(new DateTimeOffset(1976, 6, 7, 23, 17, 24,
	                                  TimeZoneInfo.FromSerializedString("Eastern Standard Time;-300;(UTC-05:00) Eastern Time (US & Canada);Eastern Standard Time;Eastern Daylight Time;[01:01:0001;12:31:2006;60;[0;02:00:00;4;1;0;];[0;02:00:00;10;5;0;];][01:01:2007;12:31:9999;60;[0;02:00:00;3;2;0;];[0;02:00:00;11;1;0;];];")
	                                              .BaseUtcOffset)) {}
}