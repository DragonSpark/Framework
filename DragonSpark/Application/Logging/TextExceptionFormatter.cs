using System;
using System.Collections.Specialized;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace DragonSpark.Application.Logging
{
	public class TextExceptionFormatter : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.TextExceptionFormatter
	{
		public TextExceptionFormatter( TextWriter writer, Exception exception ) : base( writer, exception )
		{}

		public TextExceptionFormatter( TextWriter writer, Exception exception, Guid handlingInstanceId ) : base( writer, exception, handlingInstanceId )
		{}

		/// <summary>
		/// Writes the additional properties to the <see cref="TextWriter"/>.
		/// </summary>
		/// <param name="additionalInformation">Additional information to be included with the exception report</param>
		protected override void WriteAdditionalInfo(NameValueCollection additionalInformation)
		{
			Writer.WriteLine(Resources.AdditionalInfo);
			Writer.WriteLine();

			foreach (var name in additionalInformation.AllKeys)
			{
				Writer.Write( name );
				Writer.Write( " : " );
				Writer.Write( additionalInformation[ name ] );
				Writer.Write( System.Environment.NewLine );
			}
		}
	}
}