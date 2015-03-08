using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DragonSpark.Application.Presentation.Converters
{
	public class ValidToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var realValue = (bool) value;

			if (realValue) return new SolidColorBrush(Colors.Gray);
			return new SolidColorBrush(Colors.Red);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}