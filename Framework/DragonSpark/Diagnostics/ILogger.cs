using System;
using DragonSpark.Extensions;

namespace DragonSpark.Diagnostics
{
	public interface ILogger
	{
		void Information( string message, Priority priority );

		void Warning( string message, Priority priority );

		void Exception( string message, Exception item );

		void Fatal( string message, Exception exception );
	}

	public class CompositeLogger : ILogger
	{
		readonly ILogger[] loggers;

		public CompositeLogger( params ILogger[] loggers )
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

		public void Exception( string message, Exception item )
		{
			loggers.Apply( logger => logger.Exception( message, item ) );
		}

		public void Fatal( string message, Exception exception )
		{
			loggers.Apply( logger => logger.Fatal( message, exception ) );
		}
	}
}