using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Schema;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace DragonSpark.Logging
{
	public class TraceEventListener : EventListener
	{
		readonly IEventTextFormatter formatter = new EventTextFormatter();

		protected override void OnEventWritten( EventWrittenEventArgs eventData )
		{
			var schema = EventSourceSchemaCache.Instance.GetSchema( eventData.EventId, eventData.EventSource );
			var entry = EventEntry.Create( eventData, schema );
			var message = formatter.WriteEvent( entry ).Trim();
			Trace.WriteLine( message );
		}
	}

	public class EventLogEventListener : EventListener
	{
		readonly IEventTextFormatter formatter = new EventTextFormatter();
		readonly EventLog eventLog;

		public EventLogEventListener( EventLog eventLog )
		{
			this.eventLog = eventLog;
		}

		static EventLogEntryType ToEventLogEntryType( EventLevel level )
		{
			switch ( level )
			{
				case EventLevel.Critical:
				case EventLevel.Error:
					return EventLogEntryType.Error;
				case EventLevel.Warning:
					return EventLogEntryType.Warning;
				default:
					return EventLogEntryType.Information;
			}
		}

		protected override void OnEventWritten( EventWrittenEventArgs eventData )
		{
			var schema = EventSourceSchemaCache.Instance.GetSchema( eventData.EventId, eventData.EventSource );
			var entry = EventEntry.Create( eventData, schema );
			var message = formatter.WriteEvent( entry );
			var type = ToEventLogEntryType( entry.Schema.Level );
			eventLog.WriteEntry( message, type );
		}
	}
}