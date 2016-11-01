using DragonSpark.Sources;
using System;

namespace DragonSpark.Testing.Framework
{
	public sealed class Time : SuppliedSource<DateTimeOffset>
	{
		public static Time Default { get; } = new Time();
		Time() : base( new DateTimeOffset( 1976, 6, 7, 23, 17, 57, TimeZoneInfo.FindSystemTimeZoneById( "Eastern Standard Time" ).BaseUtcOffset ) ) {}
	}
}