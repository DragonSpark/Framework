using DragonSpark.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.IO;

namespace DragonSpark.Windows.Runtime
{
	public class ExceptionFormatter : IExceptionFormatter
	{
		public static ExceptionFormatter Instance { get; } = new ExceptionFormatter();

		readonly Guid? context;

		public ExceptionFormatter() : this( null )
		{}

		public ExceptionFormatter( Guid? context )
		{
			this.context = context;
		}

		public string Format( Exception exception )
		{
			var writer = new StringWriter();
			var formatter = new TextExceptionFormatter( writer, exception, context.GetValueOrDefault() );
			formatter.Format();
			var result = writer.GetStringBuilder().ToString();
			return result;
		}
	}
}