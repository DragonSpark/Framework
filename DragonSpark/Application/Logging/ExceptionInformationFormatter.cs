using System;
using System.IO;

namespace DragonSpark.Application.Logging
{
	public class ExceptionInformationFormatter : TextExceptionFormatter
	{
		public ExceptionInformationFormatter( TextWriter writer, Exception exception ) : base( writer, exception )
		{}

		public ExceptionInformationFormatter( TextWriter writer, Exception exception, Guid handlingInstanceId ) : base( writer, exception, handlingInstanceId )
		{}

		public override void Format()
		{
			WriteMessage( Exception.Message );
			// base.Format();
		}

		/*protected override void WriteException(Exception exceptionToFormat, Exception outerException)
		{
			WriteExceptionType( exceptionToFormat.GetType() );
			WriteMessage( exceptionToFormat.Message );
		}*/
	}
}