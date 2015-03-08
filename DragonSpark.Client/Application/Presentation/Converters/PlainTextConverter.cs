using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Browser;
using System.Windows.Data;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
    public class PlainTextConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.As<string>().Transform( StripHtmlTags );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}

		static string StripHtmlTags( string value )
		{
			int length;
			int.TryParse( value, out length );

			// Remove HTML tags and empty newlines and spaces and leading spaces
			var formattedValue = Regex.Replace( value, "<.*?>", "" );
			formattedValue = Regex.Replace( formattedValue, @"\n+\s+", "\n\n" );
			formattedValue = formattedValue.TrimStart( ' ' );
			formattedValue = HttpUtility.HtmlDecode( formattedValue );
			if ( length > 0 && formattedValue.Length >= length )
			{
				formattedValue = formattedValue.Substring( 0, length - 1 );
			}
			return formattedValue;
		}
	}
}