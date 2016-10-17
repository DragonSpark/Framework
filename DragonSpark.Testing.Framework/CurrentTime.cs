using DragonSpark.Application;
using System;

namespace DragonSpark.Testing.Framework
{
	public sealed class CurrentTime : SuppliedCurrentTime
	{
		public static ICurrentTime Default { get; } = new CurrentTime();
		CurrentTime() : base( new DateTimeOffset( 1976, 6, 7, 23, 17, 57, TimeZoneInfo.FindSystemTimeZoneById( "Eastern Standard Time" ).BaseUtcOffset ) ) {}
	}
}