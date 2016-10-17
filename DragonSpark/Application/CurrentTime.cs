using DragonSpark.Sources;
using System;

namespace DragonSpark.Application
{
	public sealed class CurrentTime : SourceBase<DateTimeOffset>, ICurrentTime
	{
		public static ICurrentTime Default { get; } = new CurrentTime();
		CurrentTime() {}
		
		public override DateTimeOffset Get() => DateTimeOffset.Now;
	}
}