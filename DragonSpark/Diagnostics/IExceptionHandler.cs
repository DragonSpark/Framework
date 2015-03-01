using System;

namespace DragonSpark.Diagnostics
{
	public interface IExceptionHandler
	{
		ExceptionHandlingResult Handle( Exception exception );
	}
}