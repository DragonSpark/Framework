using System;
using System.Globalization;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class XAlignConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((global::Xamarin.Forms.TextAlignment)value)
			{
			case global::Xamarin.Forms.TextAlignment.Start:
				return System.Windows.TextAlignment.Left;
			case global::Xamarin.Forms.TextAlignment.Center:
				return System.Windows.TextAlignment.Center;
			case global::Xamarin.Forms.TextAlignment.End:
				return System.Windows.TextAlignment.Right;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
