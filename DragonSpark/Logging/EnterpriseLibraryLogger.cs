using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Logging
{
	[Singleton(typeof(ILogger))]
    public sealed class EnterpriseLibraryLogger : ILogger
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

		static LogEntry Create( object message, LoggingEntryProfile entry, Priority priority )
		{
			var result = new LogEntry( message, entry.CategorySource, (int)priority, entry.EventId.GetValueOrDefault(), entry.TraceEventType.GetValueOrDefault(), entry.Title, new Dictionary<string, object>() );
			return result;
		}

	    public void Write( string message, string category, Priority priority )
	    {
		    profiles.FirstOrDefault( x => x.CategorySource == category ).NotNull( x =>
			{
				var entry = Create( message, x, priority );
				logWriter.Write( entry );
			});
	    }
	}
}