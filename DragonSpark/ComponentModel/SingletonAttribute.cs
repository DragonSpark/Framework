using DragonSpark.Activation.IoC;
using System;

namespace DragonSpark.ComponentModel
{
	public class SingletonAttribute : DefaultValueBase
	{
		public SingletonAttribute() : this( null )
		{
		}

		public SingletonAttribute( Type hostType ) : this( hostType, "Instance" )
		{}

		public SingletonAttribute( Type hostType, string propertyName ) : base( () => new SingletonDefaultValueProvider( hostType, propertyName ) )
		{}
	}

	public class SingletonDefaultValueProvider : IDefaultValueProvider
	{
		readonly Type hostType;
		readonly string propertyName;

		public SingletonDefaultValueProvider( Type hostType, string propertyName )
		{
			this.hostType = hostType;
			this.propertyName = propertyName;
		}

		public object GetValue( DefaultValueParameter parameter )
		{
			var result = new SingletonLocator( propertyName ).Locate( hostType ?? parameter.Metadata.PropertyType );
			return result;
		}
	}
}