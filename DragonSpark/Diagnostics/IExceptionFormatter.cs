using System;

namespace DragonSpark.Diagnostics
{
	public interface IExceptionFormatter
	{
		string FormatMessage( Exception exception, Guid? contextId = null );
	}
}