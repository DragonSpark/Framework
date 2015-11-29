using System;

namespace DragonSpark.Runtime
{
	public class CurrentTime : ICurrentTime
	{
		public static CurrentTime Instance { get; } = new CurrentTime();

		CurrentTime()
		{}

		public DateTimeOffset Now => DateTimeOffset.Now;
	}
}