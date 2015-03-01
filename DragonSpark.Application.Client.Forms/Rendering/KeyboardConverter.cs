using System;
using System.Globalization;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
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
