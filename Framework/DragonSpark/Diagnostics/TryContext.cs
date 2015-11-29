using System;

namespace DragonSpark.Diagnostics
{
	public class TryContext
	{
		readonly ILogger logger;

		public TryContext( ILogger logger )
		{
			this.logger = logger;
		}

		public Exception Try( Action action )
		{
			try
			{
				action();
			}
			catch ( Exception exception )
			{
				logger.Exception( "An exception has occurred while executing an application delegate.", exception );
				return exception;
			}
			return null;
		}
	}
}