using System;

namespace DragonSpark.Diagnostics
{
	public class ApplicationExceptionFormatter : IExceptionFormatter
	{
		readonly IExceptionFormatter formatter;
		readonly ApplicationInformation information;

		public ApplicationExceptionFormatter( IExceptionFormatter formatter,  ApplicationInformation information )
		{
			this.formatter = formatter;
			this.information = information;
		}

		string IExceptionFormatter.FormatMessage( Exception exception, Guid? contextId )
		{
			return FormatMessage( exception, contextId );
		}

		protected virtual string CreateMessage( Exception exception, Guid? contextId )
		{
			var result = string.Format( "Exception occured in application {1} ({2}).{0}[Version: {3}]{0}{0}{4}{0}{5}{0}Details:{0}=============================================={0}{6}",
				Environment.NewLine,
				information.Title,
				information.Product,
				information.Version,
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

	public class ExceptionFormatter : IExceptionFormatter
	{
		public static ExceptionFormatter Instance { get; } = new ExceptionFormatter();

		public string FormatMessage( Exception exception, Guid? contextId = null )
		{
			var result = string.Concat( contextId.HasValue ? $"Context Id: {contextId}{Environment.NewLine}" : string.Empty, exception.ToString() );
			return result;
		}
	}
}