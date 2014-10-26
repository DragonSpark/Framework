using System;

namespace DragonSpark.Exceptions
{
	public interface IExceptionFormatter
	{
		string FormatMessage( Exception exception, Guid? contextId = null );
	}
}