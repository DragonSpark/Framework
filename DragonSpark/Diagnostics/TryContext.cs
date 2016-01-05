using System;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Diagnostics
{
	public class TryContext
	{
		readonly IMessageLogger messageLogger;

		public TryContext() : this( MessageLogger.Instance )
		{}

		public TryContext( [Required]IMessageLogger messageLogger )
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