using System;

namespace DragonSpark.Diagnostics
{
	public class Message
	{
		public Message( Priority priority, DateTimeOffset time, string category, string text )
		{
			Priority = priority;
			Time = time;
			Category = category;
			Text = text;
		}

		public Priority Priority { get; }

		public DateTimeOffset Time { get; }

		public string Category { get; }

		public string Text { get; }
	}
}