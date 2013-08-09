using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class DisplayIndexConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value is int ? Math.Abs( value.To<int>() ) + Math.Abs( parameter.Transform( x => System.Convert.ToInt32( x.ToString() ), () => 1 ) ) : 0;
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}