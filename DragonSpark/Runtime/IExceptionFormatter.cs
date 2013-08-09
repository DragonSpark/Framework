using System;

namespace DragonSpark.Runtime
{
	public interface IExceptionFormatter
	{
		string FormatMessage( Exception exception, Guid? contextId = null );
	}
}