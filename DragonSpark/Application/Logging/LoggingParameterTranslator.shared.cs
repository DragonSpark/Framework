using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Prism.Logging;

namespace DragonSpark.Application.Logging
{
    [Singleton( typeof(ILoggingParameterTranslator), Priority = Priority.Lowest )]
    class LoggingParameterTranslator : ILoggingParameterTranslator
    {
        readonly IEnumerable<LoggingEntryProfile> categories = LoggingDefaults.Profiles;
        readonly IEnumerable<LoggingPriorityMapping> priorities = new[]
            {
                new LoggingPriorityMapping { Priority = Priority.None, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.None },
                new LoggingPriorityMapping { Priority = Priority.Low, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.Low },
                new LoggingPriorityMapping { Priority = Priority.Lower, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.Low },
                new LoggingPriorityMapping { Priority = Priority.Lowest, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.Low },
                new LoggingPriorityMapping { Priority = Priority.BelowNormal, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.Medium },
                new LoggingPriorityMapping { Priority = Priority.Normal, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.Medium },
                new LoggingPriorityMapping { Priority = Priority.AboveNormal, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.Medium },
                new LoggingPriorityMapping { Priority = Priority.High, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.High },
                new LoggingPriorityMapping { Priority = Priority.Higher, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.High },
                new LoggingPriorityMapping { Priority = Priority.Highest, LoggingPriority = Microsoft.Practices.Prism.Logging.Priority.High },
            };

        public LoggingParameterTranslator( IEnumerable<LoggingEntryProfile> categories = null, IEnumerable<LoggingPriorityMapping> priorities = null )
        {
            this.categories = categories ?? this.categories;
            this.priorities = priorities ?? this.priorities;
        }

        public Category Translate( string category )
        {
            var result = categories.FirstOrDefault( x => string.Compare( category, x.CategorySource, StringComparison.InvariantCultureIgnoreCase ) == 0 ).Transform( x => x.Category, () => Category.Debug );
            return result;
        }

        public Microsoft.Practices.Prism.Logging.Priority Translate( Priority priority )
        {
            var result = priorities.FirstOrDefault( x => x.Priority == priority ).Transform( x => x.LoggingPriority, () => Microsoft.Practices.Prism.Logging.Priority.None );
            return result;
        }
    }
}