using System;

namespace DragonSpark.Diagnostics
{
	public class TryContext
	{
		readonly IMessageLogger messageLogger;

		public TryContext( IMessageLogger messageLogger )
		{
			this.messageLogger = messageLogger;
		}

		public Exception Try( Action action )
		{
			try
			{
				action();
			}
			catch ( Exception exception )
			{
				messageLogger.Exception( "An exception has occurred while executing an application delegate.", exception );
				return exception;
			}
			return null;
		}
	}
}