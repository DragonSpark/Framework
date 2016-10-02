using DragonSpark.Sources;
using System;

namespace DragonSpark.Windows.Runtime
{
	public class AppDomainStore<T> : AssignableSourceBase<T>
	{
		readonly AppDomain domain;
		readonly string key;

		public AppDomainStore( string key ) : this( AppDomain.CurrentDomain, key ) {}

		public AppDomainStore( AppDomain domain, string key )
		{
			this.domain = domain;
			this.key = key;
		}

		public override void Assign( T item ) => domain.SetData( key, item );

		public override T Get() => (T)domain.GetData( key );
	}
}