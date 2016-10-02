using System;

namespace DragonSpark.Application
{
	public sealed class CurrentTime : ICurrentTime
	{
		public static ICurrentTime Default { get; } = new CurrentTime();
		CurrentTime() {}
		
		public DateTimeOffset Now => DateTimeOffset.Now;
	}
}