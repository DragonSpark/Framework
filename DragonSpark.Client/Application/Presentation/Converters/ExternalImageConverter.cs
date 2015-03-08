using System;
using System.Globalization;
using System.Windows.Data;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class ExternalImageConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = Environment.IsInDesignMode ? value.As<Uri>() : value.Transform( x => ResolveHost( parameter, x ) );
			return result;
		}

		static object ResolveHost( object parameter, object value )
		{
			var uriString = new UriBuilder( System.Windows.Application.Current.GetHostUri() ) { Path = parameter.Transform( x => x.ToString(), () => "." ), Fragment = string.Empty }.Uri;
			var result = new Uri( uriString, value.To<Uri>() );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}