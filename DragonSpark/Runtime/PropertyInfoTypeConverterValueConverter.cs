using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.Reflection;

namespace DragonSpark.Runtime
{
	public class PropertyInfoTypeConverterValueConverter : TypeConverterValueConverter
	{
		static Type ResolvePropertyConverter( PropertyInfo descriptor )
		{
			var converter = descriptor.Transform( item => TypeDescriptor.GetProperties( item.DeclaringType ).Find( item.Name, false ).Converter, () => TypeDescriptor.GetConverter( descriptor.PropertyType ) );
			var result = converter.Transform( x => x.GetType() );
			return result;
		}

		public PropertyInfoTypeConverterValueConverter( PropertyInfo info ) : this( info, null )
		{}

		public PropertyInfoTypeConverterValueConverter( PropertyInfo info, Type typeConverterType ) : base( typeConverterType ?? ResolvePropertyConverter( info ) )
		{}

		/*public PropertyInfoTypeConverterValueConverter( PropertyDescriptor info ) : this( info, null )
		{}

		public PropertyInfoTypeConverterValueConverter( PropertyDescriptor info, Type typeConverterType ) : base(  )
		{}*/
	}
}
