using System;
using System.IO;
using DragonSpark.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;

namespace DragonSpark.Stationed
{
	public class EnterpriseLibraryExceptionFormatter : IExceptionFormatter
	{
		public string FormatMessage( Exception exception, Guid? contextId = null )
		{
			var writer = new StringWriter();
			new TextExceptionFormatter( writer, exception, contextId.GetValueOrDefault() ).Format();
			var result = writer.GetStringBuilder().ToString();

			/*var listener = new ObservableEventListener();
			listener.LogToConsole()*/

			return result;
		}
	}
}