using System;
using System.Runtime.Serialization;

namespace DragonSpark.Testing.TestObjects
{
	public class FatalTestingException : Exception
	{
		public FatalTestingException()
		{}

		public FatalTestingException( string message ) : base( message )
		{}

		public FatalTestingException( string message, Exception innerException ) : base( message, innerException )
		{}

		protected FatalTestingException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}
	}

	public class TestingException : Exception
	{
		public TestingException()
		{}

		public TestingException( string message ) : base( message )
		{}

		public TestingException( string message, Exception innerException ) : base( message, innerException )
		{}

		protected TestingException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}
	}
}
