using System;
using DragonSpark.TypeSystem;

namespace DragonSpark.Diagnostics
{
	public class ApplicationExceptionFormatter : IExceptionFormatter
	{
		readonly IExceptionFormatter formatter;
		readonly AssemblyInformation information;

		public ApplicationExceptionFormatter( AssemblyInformation information ) : this( ExceptionFormatter.Instance, information )
		{}

		public ApplicationExceptionFormatter( IExceptionFormatter formatter,  AssemblyInformation information )
		{
			this.formatter = formatter;
			this.information = information;
		}

		string IExceptionFormatter.FormatMessage( Exception exception )
		{
			return FormatMessage( exception );
		}

		protected virtual string CreateMessage( Exception exception )
		{
			var result = string.Format( "Exception occured in application {1} ({2}).{0}[Version: {3}]{0}{0}{4}{0}{5}{0}Details:{0}=============================================={0}{6}",
				Environment.NewLine,
				information.Title,
				information.Product,
				information.Version,
				exception.Message,
				exception.GetType(),
				formatter.FormatMessage( exception ) );
			return result;
		}

		protected virtual string FormatMessage( Exception exception )
		{
			var result = CreateMessage( exception );
			return result;
		}
	}
}