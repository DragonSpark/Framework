using System;
using System.Globalization;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class PageToRendererConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var page = value as Page;
			if ( page != null )
			{
				var result = RendererFactory.GetRenderer( page );
				page.SetRenderer( result );
				return result;
			}
			return null;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
