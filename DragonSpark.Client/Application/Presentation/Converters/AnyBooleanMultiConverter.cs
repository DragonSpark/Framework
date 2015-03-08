using System;
using System.Globalization;
using System.Linq;
using Type = System.Type;

namespace DragonSpark.Application.Presentation.Converters
{
	public class AnyBooleanMultiConverter : IMultiValueConverter
	{
		public static AnyBooleanMultiConverter Instance
		{
			get { return InstanceField; }
		}	static readonly AnyBooleanMultiConverter InstanceField = new AnyBooleanMultiConverter();

		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			var target = parameter == null || System.Convert.ToBoolean( parameter );
			var result = values.OfType<bool>().Any( x => x == target );
			return result;
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}