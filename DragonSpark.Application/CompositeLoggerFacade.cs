using DragonSpark.Extensions;
using Prism.Logging;
using System.Collections.Generic;

namespace DragonSpark.Application
{
	public class CompositeLoggerFacade : ILoggerFacade
	{
		readonly IEnumerable<ILoggerFacade> loggers;

		public CompositeLoggerFacade( params ILoggerFacade[] loggers )
		{
			this.loggers = loggers;
		}

		public void Log( string message, Category category, Prism.Logging.Priority priority )
		{
			loggers.Apply( logger => logger.Log( message, category, priority ) );
		}
	}
}