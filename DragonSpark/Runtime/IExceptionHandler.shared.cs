using System;

namespace DragonSpark.Runtime
{
	public interface IExceptionHandler
	{
		ExceptionHandlingResult Handle( Exception exception );
	}
}