using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class ImageConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			global::Xamarin.Forms.ImageSource imageSource = (global::Xamarin.Forms.ImageSource)value;
			IImageSourceHandler handler;
			if (imageSource != null && (handler = Registrar.Registered.GetHandler<IImageSourceHandler>(imageSource.GetType())) != null)
			{
				Task<System.Windows.Media.ImageSource> valueTask = handler.LoadImageAsync(imageSource, default(CancellationToken));
				return new AsyncValue<System.Windows.Media.ImageSource>(valueTask, null);
			}
			return null;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
