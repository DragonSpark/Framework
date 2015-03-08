using System;
using System.Collections.Generic;
using System.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime
{
	public static class TypeDescriptor
	{
		// Fields
		static readonly Dictionary<Type, TypeConverter> Cache = new Dictionary<Type, TypeConverter>();

		// Methods
		static TypeConverter CreateConverter(string converterTypeName)
		{
			return (System.Activator.CreateInstance(Type.GetType(converterTypeName)) as TypeConverter);
		}

		public static TypeConverter GetConverter(Type type)
		{
			TypeConverter converter;
			if (!Cache.TryGetValue(type, out converter))
			{
				var attribute = type.GetAttribute<TypeConverterAttribute>();
				Cache[type] = converter = attribute != null ? CreateConverter( attribute.ConverterTypeName ) : new ConvertibleTypeConverter();
			}
			return converter;
		}
	}
}