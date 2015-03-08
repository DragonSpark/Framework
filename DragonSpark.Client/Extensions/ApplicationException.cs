using System;

namespace DragonSpark.Extensions
{
	public class ApplicationException : Exception
	{
		public ApplicationException()
		{}

		public ApplicationException( string message ) : base( message )
		{}

		public ApplicationException( string message, Exception innerException ) : base( message, innerException )
		{}
	}
}