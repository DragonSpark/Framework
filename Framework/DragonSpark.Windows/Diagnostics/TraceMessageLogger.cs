using System.Diagnostics;
using DragonSpark.Diagnostics;
using DragonSpark.Runtime;

namespace DragonSpark.Windows.Diagnostics
{
	public class TraceMessageLogger : MessageLoggerBase
	{
		public TraceMessageLogger() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public TraceMessageLogger( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		protected override void Write( Message message )
		{
			switch ( message.Category )
			{
				case nameof(Exception):
					Trace.TraceError( message.Text );
					break;
				case nameof(Warning):
					Trace.TraceWarning( message.Text );
					break;
				default:
					Trace.TraceInformation( message.Text );
					break;
			}
		}
	}
}