using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Windows
{
	public class CompositeLoggerFacade : ILogger, IDisposable
	{
		readonly IEnumerable<ILogger> loggers;

		public CompositeLoggerFacade( params ILogger[] loggers )
		{
			this.loggers = loggers;
		}

		public void Information( string message, Priority priority )
		{
			loggers.Apply( logger => logger.Information( message, priority ) );
		}

		public void Warning( string message, Priority priority )
		{
			loggers.Apply( logger => logger.Warning( message, priority ) );
		}

		public void Exception( string message, Exception exception )
		{
			loggers.Apply( logger => logger.Exception( message, exception ) );
		}

		public void Fatal( string message, Exception exception )
		{
			loggers.Apply( logger => logger.Fatal( message, exception ) );
		}

		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
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