using DragonSpark.Extensions;
using System;

namespace DragonSpark.ComponentModel
{
	class DefaultValueProvider : IDefaultValueProvider
	{
		readonly Func<object> value;

		public DefaultValueProvider( object value ) : this( () => value ) {}

		public DefaultValueProvider( Func<object> value )
		{
			this.value = value;
		}

		public virtual object GetValue( DefaultValueParameter parameter ) => value()?.ConvertTo( parameter.Metadata.PropertyType );
	}
}