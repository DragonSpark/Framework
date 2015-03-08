using System;
using System.ComponentModel;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime
{
	partial class PropertyInfoTypeConverterValueConverter
	{
		static Type ResolvePropertyConverter( PropertyInfo descriptor )
		{
			var converter = descriptor.Transform( item => TypeDescriptor.GetProperties( item.DeclaringType ).Find( item.Name, false ).Converter, () => TypeDescriptor.GetConverter( descriptor.PropertyType ) );
			var result = converter.Transform( x => x.GetType() );
			return result;
		}
	}
}
