using System;
using System.Globalization;
using System.Windows.Media;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class ColorConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			global::Xamarin.Forms.Color color = (global::Xamarin.Forms.Color)value;
			string text = (string)parameter;
			Brush result = (text != null) ? ((Brush)System.Windows.Application.Current.Resources[text]) : new SolidColorBrush(Colors.Transparent);
			if (!(color == global::Xamarin.Forms.Color.Default))
			{
				return color.ToBrush();
			}
			return result;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
