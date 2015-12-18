using System;

namespace DragonSpark.Diagnostics
{
	public class ExceptionFormatter : IExceptionFormatter
	{
		public static ExceptionFormatter Instance { get; } = new ExceptionFormatter();

		public string FormatMessage( Exception exception )
		{
			var result = exception.ToString();
			return result;
		}
	}
}