using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class BooleanToVisibilityTranslator : IValueConverter
	{
		public static BooleanToVisibilityTranslator Instance
		{
			get { return InstanceField; }
		}	static readonly BooleanToVisibilityTranslator InstanceField = new BooleanToVisibilityTranslator();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.To<bool>() ? Visibility.Visible : Visibility.Collapsed;
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}