using System;
using System.Globalization;
using System.Windows.Data;
namespace Xamarin.Forms.Platform.WinPhone
{
	public class KeyboardConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Keyboard self = (Keyboard)value;
			return self.ToInputScope();
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
