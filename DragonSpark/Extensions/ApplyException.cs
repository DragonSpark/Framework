using System;
using System.Collections;
using System.Runtime.Serialization;

namespace DragonSpark.Extensions
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly"), Serializable]
	public class ApplyException : Exception
	{
		protected ApplyException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}

		readonly IEnumerable items;
		readonly object item;

		public ApplyException( IEnumerable items, object item, Exception innerException ) : base( "An exception occurred while applying an execution on an item.", innerException )
		{
			this.items = items;
			this.item = item;
		}

		public ApplyException( string message, Exception innerException ) : base( message, innerException )
		{}

		public ApplyException()
		{}

		public ApplyException( string message ) : base( message )
		{}

		public IEnumerable Items
		{
			get { return items; }
		}

		public object Item
		{
			get { return item; }
		}
	}
}
