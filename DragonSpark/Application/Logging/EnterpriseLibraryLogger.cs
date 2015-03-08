using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Logging
{
    public sealed class EnterpriseLibraryLogger : ILoggerFacade
	{
		readonly LogWriter logWriter;
		readonly IEnumerable<LoggingEntryProfile> profiles;

		public EnterpriseLibraryLogger( LogWriter logWriter, 
			[OptionalDependency]
			IEnumerable<LoggingEntryProfile> profiles = null )
		{
			this.logWriter = logWriter;
			this.profiles = profiles ?? LoggingDefaults.Profiles;
		}

		static LogEntry Create( object message, LoggingEntryProfile entry, Microsoft.Practices.Prism.Logging.Priority priority )
		{
			var result = new LogEntry( message, entry.CategorySource, (int)priority, entry.EventId.GetValueOrDefault(), entry.TraceEventType.GetValueOrDefault(), entry.Title, new Dictionary<string, object>() );
			return result;
		}

		public void Log( string message, Category category, Microsoft.Practices.Prism.Logging.Priority priority )
		{
			profiles.FirstOrDefault( x => x.Category == category ).NotNull( x =>
			{
				var entry = Create( message, x, priority );
				logWriter.Write( entry );
			});
		}
	}
}