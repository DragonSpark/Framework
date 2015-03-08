using System.Globalization;

namespace System.ComponentModel
{
	public class StringArrayConverter : TypeConverter
	{
		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
		{
			return ( sourceType == typeof(string) );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
		{
			if ( !( value is string ) )
			{
				throw new InvalidOperationException( );
			}
			if ( ( (string)value ).Length == 0 )
			{
				return new string[0];
			}
			var strArray = ( (string)value ).Split( new[] { ',' } );
			for ( var i = 0; i < strArray.Length; i++ )
			{
				strArray[ i ] = strArray[ i ].Trim();
			}
			return strArray;
		}

		public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value,
		                                  Type destinationType )
		{
			if ( destinationType != typeof(string) )
			{
				throw new InvalidOperationException( );
			}
			return string.Join( ",", (string[])value );
		}
	}
}