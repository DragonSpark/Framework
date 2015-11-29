using System;
using System.Reflection;
using DragonSpark.Activation.IoC;
using DragonSpark.Runtime;

namespace DragonSpark.ComponentModel
{
	public class SingletonAttribute : DefaultAttribute
	{
		readonly Type hostType;
		readonly string propertyName;

		public SingletonAttribute( Type hostType ) : this( hostType, "Instance" )
		{}

		public SingletonAttribute( Type hostType, string propertyName )
		{
			this.hostType = hostType;
			this.propertyName = propertyName;
		}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var result = new SingletonLocator( propertyName ).Locate( hostType );
			return result;
		}
	}
}