using Microsoft.Practices.Prism.Logging;
using System.Diagnostics;

namespace DragonSpark.Application
{
	public class LoggerFacade : ILoggerFacade
	{
		public void Log( string message, Category category, Microsoft.Practices.Prism.Logging.Priority priority )
		{
			Trace.WriteLine( string.Format( "Message: {0} - Category: {1} - Priority: {2}.", message, category, priority ) );
		}
	}
}