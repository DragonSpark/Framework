using System;

namespace DragonSpark.Application
{
	public partial class FatalApplicationException : Exception
	{
		public FatalApplicationException()
		{}

		public FatalApplicationException( string message ) : base( message )
		{}

		public FatalApplicationException( string message, Exception inner ) : base( message, inner )
		{}
	}
}