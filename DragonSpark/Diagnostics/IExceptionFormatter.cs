using System;

namespace DragonSpark.Diagnostics
{
	public interface IExceptionFormatter
	{
		string Format( Exception exception );
	}
}