using System;
using System.Collections;
using System.Runtime.Serialization;

namespace DragonSpark.Extensions
{
	public partial class ApplyException : Exception
	{
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