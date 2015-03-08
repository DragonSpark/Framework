using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DragonSpark.Application.Presentation.Converters
{
	public class CollapseWhenEmptyConverter : IValueConverter
	{
		public static CollapseWhenEmptyConverter Instance
		{
			get { return InstanceField; }
		}	static readonly CollapseWhenEmptyConverter InstanceField = new CollapseWhenEmptyConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var count = System.Convert.ToUInt32( value );

			return count < 1 ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}