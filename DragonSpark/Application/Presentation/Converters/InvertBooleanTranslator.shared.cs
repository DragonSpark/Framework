using System;
using System.Globalization;
using System.Windows.Data;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class InvertBooleanTranslator : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = !value.To<bool>();
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}