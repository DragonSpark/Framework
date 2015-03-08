using System.Diagnostics;
using Microsoft.Practices.Prism.Logging;

namespace DragonSpark.Application.Logging
{
    public class LoggingEntryProfile
    {
        public Category Category { get; set; }

        public string CategorySource { get; set; }
		
        public string Title { get; set; }

        public int? EventId { get; set; }

        public TraceEventType? TraceEventType { get; set; }
    }
}