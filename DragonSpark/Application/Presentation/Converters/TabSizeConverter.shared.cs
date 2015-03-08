using System;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class TabSizeConverter : ValueConverterBase<TabControl,string>
	{
		protected override object PerformConversion( TabControl value, string parameter )
		{
			var width = value.Transform( item => item.ActualWidth / item.Items.Count ); 
			int parse;
			var offset = int.TryParse(parameter, out parse) ? parse : value.Transform( item => 3 * item.Items.Count );
			var result = Math.Max( 0, width - offset );
			return result;
		}

		/*public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}*/
	}
}