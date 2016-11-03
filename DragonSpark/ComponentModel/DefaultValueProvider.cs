using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	class DefaultValueProvider : DefaultValueProviderBase
	{
		readonly Func<object> value;

		public DefaultValueProvider( object value ) : this( Factory.For( value ) ) {}

		public DefaultValueProvider( Func<object> value )
		{
			this.value = value;
		}

		public override object Get( PropertyInfo parameter ) => value()?.ConvertTo( parameter.PropertyType );
	}
}