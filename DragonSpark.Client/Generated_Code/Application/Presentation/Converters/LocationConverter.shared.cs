using System;
using System.Globalization;
using System.Windows.Data;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application.Presentation.Converters
{
    public class LocationConverter : IValueConverter
	{
		public static LocationConverter Instance
		{
			get { return InstanceField; }
		}	static readonly LocationConverter InstanceField = new LocationConverter();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var type = value.As<Type>() ?? value.As<string>().Transform( Type.GetType );
			var result = type.As<Type>().Transform( item => ServiceLocator.Current.GetInstance( item ) );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
