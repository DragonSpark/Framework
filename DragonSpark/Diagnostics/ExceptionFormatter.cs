using System;

namespace DragonSpark.Diagnostics
{
	public class ExceptionFormatter : IExceptionFormatter
	{
		readonly IExceptionFormatter formatter;
		readonly ApplicationDetails details;

		public ExceptionFormatter( IExceptionFormatter formatter,  ApplicationDetails details )
		{
			this.formatter = formatter;
			this.details = details;
		}

		string IExceptionFormatter.FormatMessage( Exception exception, Guid? contextId )
		{
			return FormatMessage( exception, contextId );
		}

		protected virtual string CreateMessage( Exception exception, Guid? contextId )
		{
			var result = string.Format( "Exception occured in application {1} ({2}).{0}[Version: {3}]{0}{0}{4}{0}{5}{0}Details:{0}=============================================={0}{6}",
				Environment.NewLine,
				details.Title,
				details.Product,
				details.Version,
				exception.Message,
				exception.GetType(),
				formatter.FormatMessage( exception, contextId ) );
			return result;
		}

		protected virtual string FormatMessage( Exception exception, Guid? contextId )
		{
			var result = CreateMessage( exception, contextId );
			return result;
		}
	}
}