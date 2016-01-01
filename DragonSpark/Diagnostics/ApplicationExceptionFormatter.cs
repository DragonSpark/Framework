using DragonSpark.TypeSystem;
using System;

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

		public string Format( Exception exception )
		{
			return PerformFormat( exception );
		}

		protected virtual string PerformFormat( Exception exception )
		{
			var result = string.Format( "Exception occured in application {1} ({2}).{0}[Version: {3}]{0}{0}{4}{0}{5}{0}Details:{0}=============================================={0}{6}",
				Environment.NewLine,
				information.Title,
				information.Product,
				information.Version,
				exception.Message,
				exception.GetType(),
				formatter.Format( exception ) );
			return result;
		}
	}
}