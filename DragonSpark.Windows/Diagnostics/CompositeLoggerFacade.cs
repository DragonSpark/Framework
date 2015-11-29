using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Diagnostics
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
			loggers.Each( logger => logger.Information( message, priority ) );
		}

		public void Warning( string message, Priority priority )
		{
			loggers.Each( logger => logger.Warning( message, priority ) );
		}

		public void Exception( string message, Exception exception )
		{
			loggers.Each( logger => logger.Exception( message, exception ) );
		}

		public void Fatal( string message, Exception exception )
		{
			loggers.Each( logger => logger.Fatal( message, exception ) );
		}

		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
			{
				loggers.OfType<IDisposable>().Each( facade => facade.Dispose() );
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