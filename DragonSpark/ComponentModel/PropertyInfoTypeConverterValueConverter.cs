using System;
using System.ComponentModel;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public class PropertyInfoTypeConverterValueConverter : TypeConverterValueConverter
	{
		static Type ResolvePropertyConverter( PropertyInfo descriptor )
		{
			var converter = descriptor.Transform( item => TypeDescriptor.GetProperties( (Type)item.DeclaringType ).Find( item.Name, false ).Converter, () => TypeDescriptor.GetConverter( descriptor.PropertyType ) );
			var result = converter.Transform( x => x.GetType() );
			return result;
		}

		public PropertyInfoTypeConverterValueConverter( PropertyInfo info ) : this( info, null )
		{}

		public PropertyInfoTypeConverterValueConverter( PropertyInfo info, Type typeConverterType ) : base( typeConverterType ?? ResolvePropertyConverter( info ) )
		{}
	}
}
