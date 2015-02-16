using System;
using System.Globalization;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class CaseConverter : System.Windows.Data.IValueConverter
	{
		public bool ConvertToUpper
		{
			get;
			set;
		}
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}
			var text = (string)value;
			var result = !ConvertToUpper ? text.ToLower() : text.ToUpper();
			return result;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
