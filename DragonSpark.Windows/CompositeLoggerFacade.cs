using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Logging;

namespace DragonSpark.Windows
{
	public class CompositeLoggerFacade : ILoggerFacade, IDisposable
	{
		readonly IEnumerable<ILoggerFacade> loggers;

		public CompositeLoggerFacade( params ILoggerFacade[] loggers )
		{
			this.loggers = loggers;
		}

		public void Log( string message, Category category, DragonSpark.Logging.Priority priority )
		{
			loggers.Apply( logger => logger.Log( message, category, priority ) );
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				loggers.OfType<IDisposable>().Apply( facade => facade.Dispose() );
			}
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		/// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}