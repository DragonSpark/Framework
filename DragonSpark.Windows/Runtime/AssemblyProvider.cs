using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : AssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		protected override Assembly[] DetermineAll()
		{
			var result = AllClasses.FromAssembliesInBasePath( includeUnityAssemblies: true )
				.Where( x => x.Namespace != null )
				.GroupBy( type => type.Assembly )
				.Select( types => types.Key ).ToArray();
			return result;
		}
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
			AppDomain.CurrentDomain.SetData( key, item );
		}

		public override T Item => (T)domain.GetData( key );
	}
}