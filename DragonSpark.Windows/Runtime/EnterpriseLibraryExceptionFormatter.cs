using DragonSpark.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.IO;

namespace DragonSpark.Windows.Runtime
{
	public class EnterpriseLibraryExceptionFormatter : IExceptionFormatter
	{
		public static EnterpriseLibraryExceptionFormatter Instance { get; } = new EnterpriseLibraryExceptionFormatter();

		readonly Guid? context;

		public EnterpriseLibraryExceptionFormatter() : this( null )
		{}

		public EnterpriseLibraryExceptionFormatter( Guid? context )
		{
			this.context = context;
		}

		public string Format( Exception exception )
		{
			var writer = new StringWriter();
			var formatter = context.HasValue ? new TextExceptionFormatter( writer, exception, context.Value ) : new TextExceptionFormatter( writer, exception );
			formatter.Format();
			var result = writer.GetStringBuilder().ToString();
			return result;
		}
	}
}