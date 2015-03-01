using System.Windows;
using DragonSpark.Application.Client.Converters;
using DragonSpark.Extensions;
using Xamarin.Forms;
using Image = System.Windows.Controls.Image;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class ImageConverter : ConverterBase<ImageSource>
	{
		public static ImageConverter Instance
		{
			get { return InstanceField; }
		}	static readonly ImageConverter InstanceField = new ImageConverter();

		protected override object PerformConversion( ImageSource value, object parameter )
		{
			var result = Handle( value );
			return result;
		}

		internal static AsyncValue<System.Windows.Media.ImageSource> Handle( ImageSource value )
		{
			var task = Registrar.Registered.GetHandler<IImageSourceHandler>( value.GetType() ).Transform( handler => handler.LoadImageAsync( value ) );
			var result = task.Transform( item => new AsyncValue<System.Windows.Media.ImageSource>( item, null ) );
			return result;
		}
	}

	/*public static class Images
	{
		public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached( "Source", typeof(ImageSource), typeof(Images), new PropertyMetadata( OnSourcePropertyChanged ) );

		static void OnSourcePropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{
			o.As<Image>( image => e.NewValue.As<ImageSource>( source =>
			{
				ImageConverter.Handle( source )
			} ) );
		}

		public static ImageSource GetSource( Image element )
		{
			return (ImageSource)element.GetValue( SourceProperty );
		}

		public static void SetSource( FrameworkElement element, ImageSource value )
		{
			element.SetValue( SourceProperty, value );
		}
	}*/
}
