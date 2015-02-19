using System;

namespace DragonSpark.Client
{
	public class FatalApplicationException : Exception
	{
		public FatalApplicationException()
		{}

		public FatalApplicationException( string message ) : base( message )
		{}

		public FatalApplicationException( string message, Exception inner ) : base( message, inner )
		{}
	}
}