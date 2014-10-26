using System;
using System.IO;
using DragonSpark.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DragonSpark.Testing.Server
{
	public class EnterpriseLibraryExceptionFormatter : IExceptionFormatter
	{
		public string FormatMessage( Exception exception, Guid? contextId = null )
		{
			var writer = new StringWriter();
			new TextExceptionFormatter( writer, exception, contextId.GetValueOrDefault() ).Format();
			var result = writer.GetStringBuilder().ToString();
			return result;
		}
	}
}