using System;
using System.Globalization;
using System.Linq;
using Type = System.Type;

namespace DragonSpark.Application.Presentation.Converters
{
	public class AllBooleanMultiConverter : IMultiValueConverter
	{
		public static AllBooleanMultiConverter Instance
		{
			get { return InstanceField; }
		}	static readonly AllBooleanMultiConverter InstanceField = new AllBooleanMultiConverter();

		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			var target = parameter == null || System.Convert.ToBoolean( parameter );
			var result = values.OfType<bool>().All( x => x == target );
			return result;
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}