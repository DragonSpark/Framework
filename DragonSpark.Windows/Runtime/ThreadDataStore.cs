using DragonSpark.Sources;
using System;
using System.Threading;

namespace DragonSpark.Windows.Runtime
{
	public class ThreadDataStore<T> : AssignableSourceBase<T>
	{
		readonly LocalDataStoreSlot slot;

		public ThreadDataStore( string key ) : this( Thread.GetNamedDataSlot( key ) ) {}

		public ThreadDataStore( LocalDataStoreSlot slot )
		{
			this.slot = slot;
		}

		public override void Assign( T item ) => Thread.SetData( slot, item );

		public override T Get() => (T)Thread.GetData( slot );
	}
}