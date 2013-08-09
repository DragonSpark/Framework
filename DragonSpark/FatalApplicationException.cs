using System;
using System.Runtime.Serialization;

namespace DragonSpark
{
	[Serializable]
	public class FatalApplicationException : Exception
	{
		public FatalApplicationException()
		{}

		public FatalApplicationException( string message ) : base( message )
		{}

		public FatalApplicationException( string message, Exception inner ) : base( message, inner )
		{}

		protected FatalApplicationException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}

	}
}