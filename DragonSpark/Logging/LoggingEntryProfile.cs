using System.Diagnostics;

namespace DragonSpark.Logging
{
    public class LoggingEntryProfile
    {
        public string CategorySource { get; set; }
		
        public string Title { get; set; }

        public int? EventId { get; set; }

        public TraceEventType? TraceEventType { get; set; }
    }
}