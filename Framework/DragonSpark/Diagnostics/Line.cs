using System;

namespace DragonSpark.Diagnostics
{
	public class Line
	{
		public Line( Priority priority, DateTimeOffset time, string category, string message )
		{
			Priority = priority;
			Time = time;
			Category = category;
			Message = message;
		}

		public Priority Priority { get; }

		public DateTimeOffset Time { get; }

		public string Category { get; }

		public string Message { get; }
	}
}