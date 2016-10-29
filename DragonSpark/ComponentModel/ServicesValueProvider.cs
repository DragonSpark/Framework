using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ServicesValueProvider : DefaultValueProviderBase
	{
		readonly Func<PropertyInfo, Type> convert;
		readonly Func<Type, object> create;

		public ServicesValueProvider( Func<PropertyInfo, Type> convert, Func<Type, object> create )
		{
			this.convert = convert;
			this.create = create;
		}

		public override object Get( PropertyInfo parameter ) => create( convert( parameter ) );
	}
}