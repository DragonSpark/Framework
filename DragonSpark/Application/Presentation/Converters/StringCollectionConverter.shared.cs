using System.Collections.Generic;

namespace DragonSpark.Application.Presentation.Converters
{
	/*public class HiddenWhenHigherConverter : BooleanConverter
	{
		protected override bool Locate(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var count = (int)value;
			var min = System.Convert.ToInt32(parameter);
			return count > min
					   ? Visibility.Hidden
					   : Visibility.Visible;
		}
	}
	*/

	public class StringCollectionConverter : ValueConverterBase<IEnumerable<string>,object>
	{
		public static StringCollectionConverter Instance
		{
			get { return InstanceField; }
		}	static readonly StringCollectionConverter InstanceField = new StringCollectionConverter();

		protected override object PerformConversion( IEnumerable<string> value, object parameter )
		{
			var result = string.Join( System.Environment.NewLine, value );
			return result;
		}
	}

	/*public class BooleanConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var result = value.To<bool>();
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}*/
}