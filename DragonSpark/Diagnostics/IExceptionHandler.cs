using System;

namespace DragonSpark.Diagnostics
{
	public interface IExceptionHandler
	{
		ExceptionHandlingResult Handle( Exception exception );
	}

	class ExceptionHandler : IExceptionHandler
	{
		public static ExceptionHandler Instance { get; } = new ExceptionHandler();

		ExceptionHandler()
		{}

		public ExceptionHandlingResult Handle( Exception exception )
		{
			var result = new ExceptionHandlingResult( true, exception );
			return result;
		}
	}
}