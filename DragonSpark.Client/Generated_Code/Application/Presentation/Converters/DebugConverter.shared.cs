using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace DragonSpark.Application.Presentation.Converters
{
	public class DebugConverter : IValueConverter
	{
		public static DebugConverter Instance
		{
			get { return InstanceField; }
		}	static readonly DebugConverter InstanceField = new DebugConverter();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			Debugger.Break();
			return value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			Debugger.Break();
			return value;
		}
	}
}