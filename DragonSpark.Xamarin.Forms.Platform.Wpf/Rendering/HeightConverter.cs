using System;
using System.Globalization;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class HeightConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double num = (double)value;
			return (num > 0.0) ? num : double.NaN;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
