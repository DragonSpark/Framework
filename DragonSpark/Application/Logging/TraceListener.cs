using System.Collections.ObjectModel;
using System.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace DragonSpark.Application.Logging
{
	[ConfigurationElementType(typeof(CustomTraceListenerData))]
	public class TraceListener : CustomTraceListener, ILoggingOutput
	{
		readonly ObservableCollection<string> outputSource = new ObservableCollection<string>();
		readonly ObservableCollection<LogEntry> logSource = new ObservableCollection<LogEntry>();

		public TraceListener()
		{
			output = new ReadOnlyObservableCollection<string>( outputSource );
			log = new ReadOnlyObservableCollection<LogEntry>( logSource );
		}

		public ReadOnlyObservableCollection<string> Output
		{
			get { return output; }
		}	readonly ReadOnlyObservableCollection<string> output;

		public ReadOnlyObservableCollection<LogEntry> Log
		{
			get { return log; }
		}	readonly ReadOnlyObservableCollection<LogEntry> log;

		public override void TraceData( TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data )
		{
			var line = data.As<LogEntry>( item => logSource.Add( item ) ).Transform( entry => Formatter.Transform( item => item.Format( entry ) ), data.ToString );
			outputSource.Add( line );
			WriteLine( line );
		}

		public override void Write( string message )
		{
			Trace.Write( message );
		}

		public override void WriteLine( string message )
		{
			// Delimit each message
			Attributes[ "delimiter" ].NotNull( x => Trace.WriteLine( x ) );
			
			// Write formatted message
			Trace.WriteLine( message );
		}

		public void Reset()
		{
			outputSource.Clear();
		}
	}
}