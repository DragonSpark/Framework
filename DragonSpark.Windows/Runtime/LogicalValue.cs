using DragonSpark.Runtime.Values;
using System.Runtime.Remoting.Messaging;

namespace DragonSpark.Windows.Runtime
{
	public class LogicalValue<T> : WritableValue<T>
	{
		readonly string slot;

		public LogicalValue( string slot )
		{
			this.slot = slot;
		}

		public override void Assign( T item )
		{
			if ( item == null )
			{
				CallContext.FreeNamedDataSlot( slot );
			}
			else
			{
				CallContext.LogicalSetData( slot, item );
			}
		}

		public override T Item => (T)CallContext.LogicalGetData( slot );
	}
}