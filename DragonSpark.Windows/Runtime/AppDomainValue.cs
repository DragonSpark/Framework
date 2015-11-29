using System;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Windows.Runtime
{
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
			AppDomain.CurrentDomain.SetData( key, item );
		}

		public override T Item => (T)domain.GetData( key );
	}
}