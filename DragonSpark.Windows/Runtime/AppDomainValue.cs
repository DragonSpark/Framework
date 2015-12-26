using DragonSpark.Runtime.Values;
using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace DragonSpark.Windows.Runtime
{
	public class ThreadLocalValue<T> : WritableValue<T>
	{
		readonly LocalDataStoreSlot slot;

		public ThreadLocalValue( string key ) : this( Thread.GetNamedDataSlot( key ) )
		{}

		public ThreadLocalValue( LocalDataStoreSlot slot )
		{
			this.slot = slot;
		}

		public override void Assign( T item )
		{
			Thread.SetData( slot, item );
		}

		public override T Item => (T)Thread.GetData( slot );
	}

	public class LogicalValue<T> : WritableValue<T>
	{
		readonly string slot;

		public LogicalValue( string slot )
		{
			this.slot = slot;
		}

		public override void Assign( T item )
		{
			CallContext.LogicalSetData( slot, item );
		}

		public override T Item => (T)CallContext.LogicalGetData( slot );
	}

	public class AppDomainValue<T> : WritableValue<T>
	{
		readonly AppDomain domain;
		readonly string key;

		public AppDomainValue( string key ) : this( AppDomain.CurrentDomain, key )
		{}

		public AppDomainValue( AppDomain domain, string key )
		{
			this.domain = domain;
			this.key = key;
		}

		public override void Assign( T item )
		{
			domain.SetData( key, item );
		}

		public override T Item => (T)domain.GetData( key );
	}
}