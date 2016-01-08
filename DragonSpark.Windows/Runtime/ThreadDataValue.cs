using DragonSpark.Runtime.Values;
using System;
using System.Threading;

namespace DragonSpark.Windows.Runtime
{
	public class ThreadDataValue<T> : WritableValue<T>
	{
		readonly LocalDataStoreSlot slot;

		public ThreadDataValue( string key ) : this( Thread.GetNamedDataSlot( key ) )
		{}

		public ThreadDataValue( LocalDataStoreSlot slot )
		{
			this.slot = slot;
		}

		public override void Assign( T item ) => Thread.SetData( slot, item );

		public override T Item => (T)Thread.GetData( slot );
	}
}