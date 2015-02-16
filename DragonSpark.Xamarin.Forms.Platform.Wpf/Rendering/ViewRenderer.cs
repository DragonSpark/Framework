using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class ViewRenderer<TElement, TNativeElement> : VisualElementRenderer<TElement, TNativeElement> where TElement : View where TNativeElement : FrameworkElement
	{
	}
	public class ViewRenderer : ViewRenderer<View, FrameworkElement>
	{
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Layout.IsClippedToBoundsProperty.PropertyName)
			{
				this.UpdateClipToBounds();
			}
		}
		protected override void UpdateNativeWidget()
		{
			base.UpdateNativeWidget();
			this.UpdateClipToBounds();
		}
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);
			base.SizeChanged += delegate(object sender, SizeChangedEventArgs args)
			{
				this.UpdateClipToBounds();
			};
			this.UpdateBackgroundColor();
		}
		private void UpdateClipToBounds()
		{
			Layout layout = base.Element as Layout;
			if (layout != null)
			{
				base.Clip = null;
				if (layout.IsClippedToBounds)
				{
					base.Clip = new RectangleGeometry
					{
						Rect = new Rect(0.0, 0.0, base.ActualWidth, base.ActualHeight)
					};
				}
			}
		}
	}
}
