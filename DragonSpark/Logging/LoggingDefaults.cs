using System.Collections.Generic;
using System.Diagnostics;

namespace DragonSpark.Logging
{
    static class LoggingDefaults
    {
        static readonly IEnumerable<LoggingEntryProfile> ProfilesField = new[]
            {
                new LoggingEntryProfile { CategorySource = "Debug", EventId = 0, TraceEventType = TraceEventType.Verbose },
                new LoggingEntryProfile { CategorySource = "Information", EventId = 0, TraceEventType = TraceEventType.Information },
                new LoggingEntryProfile { CategorySource = "Warning", EventId = 0, TraceEventType = TraceEventType.Warning },
                new LoggingEntryProfile { CategorySource = "Error", EventId = 0, TraceEventType = TraceEventType.Error }
            };

        public static IEnumerable<LoggingEntryProfile> Profiles
        {
            get { return ProfilesField; }
        }
    }
}