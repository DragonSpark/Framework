using System;
using System.IO;

namespace DragonSpark.Logging
{
	public class TextExceptionFormatter : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.TextExceptionFormatter
	{
		public TextExceptionFormatter(TextWriter writer, Exception exception) : base(writer, exception)
		{}

		public TextExceptionFormatter(TextWriter writer, Exception exception, Guid handlingInstanceId) : base(writer, exception, handlingInstanceId)
		{}

		protected override void WriteException( Exception exceptionToFormat, Exception outerException )
		{
			var exception = outerException ?? exceptionToFormat;
			var message = ExceptionMessageProvider.Instance.GetMessage(exception);
			Writer.WriteLine(message);
		}
	}
}