using System;

namespace DragonSpark.Runtime
{
	public interface ICurrentTime
	{
		DateTimeOffset Now { get; }
	}

	public class CurrentTime : ICurrentTime
	{
		public static CurrentTime Instance { get; } = new CurrentTime();

		CurrentTime()
		{}

		public DateTimeOffset Now => DateTimeOffset.Now;
	}
}
