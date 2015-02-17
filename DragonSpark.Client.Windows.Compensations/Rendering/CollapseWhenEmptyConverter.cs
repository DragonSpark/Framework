using System;
using System.Globalization;
using System.Windows;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class CollapseWhenEmptyConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int num = 0;
			string text = value as string;
			if (text != null)
			{
				num = text.Length;
			}
			if (value is int)
			{
				num = (int)value;
			}
			return (num > 0) ? Visibility.Visible : Visibility.Collapsed;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
