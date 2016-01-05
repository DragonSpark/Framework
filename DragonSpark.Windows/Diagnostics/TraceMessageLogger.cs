using DragonSpark.Diagnostics;
using System.Diagnostics;

namespace DragonSpark.Windows.Diagnostics
{
	public class TraceMessageLogger : MessageLoggerBase
	{
		protected override void OnLog( Message message )
		{
			switch ( message.Category )
			{
				case ExceptionMessageFactory.Category:
					Trace.TraceError( message.Text );
					break;
				case WarningMessageFactory.Category:
					Trace.TraceWarning( message.Text );
					break;
				default:
					Trace.TraceInformation( message.Text );
					break;
			}
		}
	}
}