using System;

namespace DragonSpark.Runtime
{
	public class ExceptionHandlingResult
	{
		public ExceptionHandlingResult( bool handled, Exception exception )
		{
			Handled = handled;
			Exception = exception;
		}

		public bool Handled { get; private set; }

		public Exception Exception { get; private set; }
	}
}