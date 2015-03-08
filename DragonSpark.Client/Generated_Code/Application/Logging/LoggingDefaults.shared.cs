using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.Prism.Logging;

namespace DragonSpark.Application.Logging
{
    static class LoggingDefaults
    {
        static readonly IEnumerable<LoggingEntryProfile> ProfilesField = new[]
            {
                new LoggingEntryProfile { Category = Category.Debug, CategorySource = "Debug", EventId = 0, TraceEventType = TraceEventType.Verbose },
                new LoggingEntryProfile { Category = Category.Info, CategorySource = "Information", EventId = 0, TraceEventType = TraceEventType.Information },
                new LoggingEntryProfile { Category = Category.Warn, CategorySource = "Warning", EventId = 0, TraceEventType = TraceEventType.Warning },
                new LoggingEntryProfile { Category = Category.Exception, CategorySource = "Error", EventId = 0, TraceEventType = TraceEventType.Error }
            };

        public static IEnumerable<LoggingEntryProfile> Profiles
        {
            get { return ProfilesField; }
        }
    }
}