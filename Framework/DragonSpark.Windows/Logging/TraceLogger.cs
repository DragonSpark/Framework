using DragonSpark.Diagnostics;
using DragonSpark.Runtime;
using System.Diagnostics;

namespace DragonSpark.Windows.Logging
{
	public class TraceLogger : LoggerBase
	{
		public TraceLogger() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public TraceLogger( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		protected override void Write( Line line )
		{
			switch ( line.Category )
			{
				case nameof(Exception):
					Trace.TraceError( line.Message );
					break;
				case nameof(Warning):
					Trace.TraceWarning( line.Message );
					break;
				default:
					Trace.TraceInformation( line.Message );
					break;
			}
		}
	}
}