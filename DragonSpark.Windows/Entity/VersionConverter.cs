using System;
using System.ComponentModel;
using System.Globalization;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Entity
{
	public class VersionConverter : TypeConverter
	{
		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
		{
			return ( sourceType == typeof(string) );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
		{
			if ( !( value is string ) )
			{
				throw new InvalidOperationException( "This converter expects strings." );
			}
			var result = value.As<string>().Transform( x => new Version( x ) );
			return result;
		}

		public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value,
			Type destinationType )
		{
			if ( destinationType != typeof(string) )
			{
				throw new InvalidOperationException( );
			}
			var result = value.As<Version>().Transform( x => x.ToString() );
			return result;
		}
	}
}