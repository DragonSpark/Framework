using System;
using System.Globalization;
using System.Windows.Data;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class StringFormatConverter : IValueConverter
	{
		public static StringFormatConverter Instance
		{
			get { return InstanceField; }
		}	static readonly StringFormatConverter InstanceField = new StringFormatConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = string.Format( parameter.Transform( x => x.ToString(), () => "{0}" ), value);
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}