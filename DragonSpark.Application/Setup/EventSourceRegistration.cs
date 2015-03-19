using System.Diagnostics.Tracing;

namespace DragonSpark.Application.Setup
{
	public class EventSourceRegistration
	{
		public EventSourceRegistration()
		{
			Level = EventLevel.LogAlways;
			Keywords = (EventKeywords)(-1);
		}

		public EventLevel Level { get; set; }

		public EventKeywords Keywords { get; set; }
	}
}