using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace DragonSpark.Runtime
{
	public static class ConversionSupport
	{
		public static TResult ConvertTo<TResult>( this object target )
		{
			var result = target.Transform( x => ConvertTo( x, typeof(TResult) ).To<TResult>() );
			return result;
		}

		public static object ConvertTo( this object source, Type targetType, Type typeConverterType = null )
		{
			var converter = new TypeConverterValueConverter( typeConverterType );
			var result = converter.Convert( source, targetType, null, CultureInfo.CurrentCulture );
			return result;
		}

		public static object ConvertTo( this object source, PropertyInfo targetProperty, Type typeConverterType )
		{
			var converter = new PropertyInfoTypeConverterValueConverter( targetProperty, typeConverterType );
			var result = converter.Convert( source, targetProperty.PropertyType, null, CultureInfo.CurrentCulture );
			return result;
		}

		public static bool IsConvertible( Type from, Type to )
		{
			TypeConverter converterFrom = TypeDescriptor.GetConverter( from ),
			              converterTo = TypeDescriptor.GetConverter( to );
			var result = from.IsAssignableFrom( to ) || converterFrom.Transform( x => x.CanConvertTo( to ) ) || converterTo.Transform( x => x.CanConvertFrom( from ) );
			return result;
		}
	}
}