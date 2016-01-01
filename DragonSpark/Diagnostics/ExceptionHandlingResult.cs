using System;

namespace DragonSpark.Diagnostics
{
	public class ExceptionHandlingResult
	{
		public ExceptionHandlingResult( bool rethrowRecommended, Exception exception )
		{
			RethrowRecommended = rethrowRecommended;
			Exception = exception;
		}

		public bool RethrowRecommended { get; }

		public Exception Exception { get; }
	}
}