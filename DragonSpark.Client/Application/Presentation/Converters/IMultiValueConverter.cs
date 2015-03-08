using System.Globalization;

namespace DragonSpark.Application.Presentation.Converters
{
	public interface IMultiValueConverter
	{
		object Convert( object[] values, System.Type targetType, object parameter, CultureInfo culture );

		object[] ConvertBack( object value, System.Type[] targetTypes, object parameter, CultureInfo culture );
	}
}