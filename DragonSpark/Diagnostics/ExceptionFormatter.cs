using System;

namespace DragonSpark.Diagnostics
{
	public class ExceptionFormatter : IExceptionFormatter
	{
		public static ExceptionFormatter Instance { get; } = new ExceptionFormatter();

		public string Format( Exception exception )
		{
			var result = exception.ToString();
			return result;
		}
	}
}