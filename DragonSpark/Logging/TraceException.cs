using System;
using System.Linq;
using System.Runtime.Serialization;
using DragonSpark.Extensions;

namespace DragonSpark.Logging
{
	[Serializable, System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1058:TypesShouldNotExtendCertainBaseTypes", Justification = "This exception is used in application-specific scenarios." )]
	public class TraceException : ApplicationException
	{
		public TraceException( ApplicationDetails details, Exception innerException ) : base( "An application ", innerException )
		{
			innerException.Data.Keys.Cast<object>().Apply( x => Data.Add( x, innerException.Data[x] ) );
		}

		protected TraceException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}

		public TraceException()
		{}

		public TraceException( string message ) : base( message )
		{}
	}
}