using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using Image = Xamarin.Forms.Image;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class ImageRenderer : ViewRenderer<Image, System.Windows.Controls.Image>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);
			System.Windows.Controls.Image image = new System.Windows.Controls.Image();
			this.SetSource(image);
			this.SetAspect(image);
			base.SetNativeControl(image);
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == global::Xamarin.Forms.Image.SourceProperty.PropertyName)
			{
				this.SetSource(base.Control);
				return;
			}
			if (e.PropertyName == global::Xamarin.Forms.Image.AspectProperty.PropertyName)
			{
				this.SetAspect(base.Control);
			}
		}
		private async void SetSource(System.Windows.Controls.Image image)
		{
			((IElementController)base.Element).SetValueFromRenderer(global::Xamarin.Forms.Image.IsLoadingPropertyKey, true);
			global::Xamarin.Forms.ImageSource source = base.Element.Source;
			IImageSourceHandler handler;
			if (source != null && (handler = Registrar.Registered.GetHandler<IImageSourceHandler>(source.GetType())) != null)
			{
				System.Windows.Media.ImageSource source2;
				try
				{
					source2 = await handler.LoadImageAsync(source, default(CancellationToken));
				}
				catch (TaskCanceledException)
				{
					source2 = null;
				}
				image.Source = source2;
				((IVisualElementController)base.Element).NativeSizeChanged();
			}
			else
			{
				image.Source = null;
			}
			((IElementController)base.Element).SetValueFromRenderer(global::Xamarin.Forms.Image.IsLoadingPropertyKey, false);
		}
		private void SetAspect(System.Windows.Controls.Image image)
		{
			Aspect aspect = base.Element.Aspect;
			image.Stretch = aspect.ToStretch();
		}
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (base.Control.Source == null)
			{
				return default(SizeRequest);
			}
			Size request = new Size
			{
				Width = (double)((BitmapImage)base.Control.Source).PixelWidth,
				Height = (double)((BitmapImage)base.Control.Source).PixelHeight
			};
			return new SizeRequest(request);
		}
	}
}
