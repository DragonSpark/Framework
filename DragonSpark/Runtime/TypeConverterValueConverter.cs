using DragonSpark.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Windows.Data;

namespace DragonSpark.Runtime
{
	public class TypeConverterValueConverter : IValueConverter
	{
		readonly Type typeConverterType;

		public TypeConverterValueConverter() : this( null )
		{}

		public TypeConverterValueConverter( Type typeConverterType )
		{
			this.typeConverterType = typeConverterType;
		}

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.Transform( x => Convert( value, targetType ), () => value );
			return result;
		}

		public static object GetDefaultValue(Type type)
		{
			Contract.Requires( type != null );
			return type.IsClass || type.IsInterface ? null : Activator.CreateInstance<object>(type);
		}

		object Convert( object value, Type targetType )
		{
			Contract.Requires( targetType != null );
			if ( value != null )
			{
				var providedType = value.GetType();
				var assigned = targetType.IsAssignableFrom( providedType ).Transform( x => value );
				var result = typeConverterType != null ? ResolveFromConverter( value, targetType, providedType ) ?? assigned ?? ChangeType( value, targetType )
					             : 
					             assigned ?? ResolveFromConverter( value, targetType, providedType ) ?? ChangeType( value, targetType );
				return result;
			}
			return GetDefaultValue( targetType );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		static object ChangeType( object value, Type targetType )
		{
			try
			{
				return System.Convert.ChangeType( value, targetType, CultureInfo.CurrentUICulture );
			}
			catch
			{
				return ParseSupport.TryParse( value, targetType ) ?? GetDefaultValue( targetType );
			}
		}


		static class ParseSupport
		{
			public static object TryParse( object value, Type targetType )
			{
				var parameters = new[] { value, null };
				value.NotNull( x => targetType.GetMethod( "TryParse", new[] { x.GetType(), targetType.MakeByRefType() } ).NotNull( y => y.Invoke( null, parameters ) ) );
				return parameters[1];
			}
		}

		object ResolveFromConverter( object value, Type targetType, Type providedType )
		{
			using ( ResolveConverter( targetType ) )
			{
				var converter = TypeDescriptor.GetConverter( targetType );
				if ( converter.CanConvertFrom( providedType ) )
				{
					return converter.ConvertFrom( value );
				}

				converter = TypeDescriptor.GetConverter( providedType );
				if ( converter.CanConvertTo( targetType ) )
				{
					return converter.ConvertTo( value, targetType );
				}
			}
			return null;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.Transform<object, object>( x => Convert( value, targetType ) );
			return result;
		}

		IDisposable ResolveConverter( Type targetType )
		{
			var result = targetType.Transform( x => new TypeConverterManager( targetType, typeConverterType ) );
			return result;
		}
	}
}
