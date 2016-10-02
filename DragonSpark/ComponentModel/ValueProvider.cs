using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ValueProvider<TRequest> : DefaultValueProviderBase
	{
		readonly Func<PropertyInfo, TRequest> convert;
		readonly Func<TRequest, object> create;

		public ValueProvider( Func<PropertyInfo, TRequest> convert, Func<TRequest, object> create )
		{
			this.convert = convert;
			this.create = create;
		}

		public override object Get( DefaultValueParameter parameter ) => create( convert( parameter.Metadata ) );
	}
}