using DragonSpark.Extensions;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using IValueConverter = System.Windows.Data.IValueConverter;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class ImageConverter : IValueConverter
	{
		public static ImageConverter Instance
		{
			get { return InstanceField; }
		}	static readonly ImageConverter InstanceField = new ImageConverter();

		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			var task = value.AsTo<ImageSource, Task<System.Windows.Media.ImageSource>>( source => Registrar.Registered.GetHandler<IImageSourceHandler>( source.GetType() ).Transform( handler => handler.LoadImageAsync( source )  ) );
			var result = task.Transform( item => new AsyncValue<System.Windows.Media.ImageSource>( item, null ) );
			return result;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}
