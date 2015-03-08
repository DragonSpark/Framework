using System;
using System.Linq;
using System.Runtime.Serialization;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Logging
{
	[Serializable, System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1058:TypesShouldNotExtendCertainBaseTypes", Justification = "This exception is used in application-specific scenarios." )]
	public class LoggingInformationException : ApplicationException
	{
		public LoggingInformationException( string message, Exception innerException ) : base( message, innerException )
		{
			innerException.Data.Keys.Cast<object>().Apply( x => Data.Add( x, innerException.Data[x] ) );
		}

		protected LoggingInformationException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}

		public LoggingInformationException()
		{}

		public LoggingInformationException( string message ) : base( message )
		{}
	}
}