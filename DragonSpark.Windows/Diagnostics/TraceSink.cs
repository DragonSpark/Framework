using System.Diagnostics;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace DragonSpark.Windows.Diagnostics
{
	sealed class TraceSink : ILogEventSink
	{
		readonly ITextFormatter textFormatter;

		public TraceSink( ITextFormatter textFormatter )
		{
			this.textFormatter = textFormatter;
		}

		public void Emit( LogEvent logEvent )
		{
			var stringWriter = new StringWriter();
			textFormatter.Format( logEvent, stringWriter );
			var message = stringWriter.ToString().Trim();
			Trace.WriteLine( message );
		}
	}
}