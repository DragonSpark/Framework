using DragonSpark.Sources;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace DragonSpark.Windows.Runtime
{
	public class LogicalStore<T> : AssignableSourceBase<T>
	{
		readonly string slot;

		public LogicalStore( string slot )
		{
			this.slot = slot;
		}

		public override void Assign( [Optional]T item )
		{
			if ( item == null )
			{
				CallContext.FreeNamedDataSlot( slot );
			}
			else
			{
				CallContext.SetData( slot, item );
			}
		}

		public override T Get() => (T)CallContext.GetData( slot );
	}
}