using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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

	public class TestEventListener : EventListener
	{
		public EventLevel Level { get; set; }

		public ICollection<EventWrittenEventArgs> Items
		{
			get { return items; }
		}	readonly ICollection<EventWrittenEventArgs> items = new List<EventWrittenEventArgs>();

		protected override void OnEventWritten( EventWrittenEventArgs eventData )
		{
			if ( eventData.Level == Level )
			{
				Items.Add( eventData );
			}
		}
	}
}
