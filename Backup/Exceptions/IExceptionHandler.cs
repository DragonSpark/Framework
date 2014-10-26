using System;

namespace DragonSpark.Exceptions
{
	public interface IExceptionHandler
	{
		ExceptionHandlingResult Handle( Exception exception );
	}
}