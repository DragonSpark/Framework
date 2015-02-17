using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public class ScrollViewRenderer : ViewRenderer<ScrollView, ScrollViewer>
	{
		public ScrollViewRenderer()
		{
			base.AutoPackage = false;
		}
		protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
		{
			base.OnElementChanged(e);
			base.SetNativeControl(new ScrollViewer());
			base.SizeChanged += delegate(object sender, SizeChangedEventArgs args)
			{
				base.Control.Width = base.ActualWidth;
				base.Control.Height = base.ActualHeight;
			};
			this.UpdateOrientation();
			this.LoadContent();
		}
		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			SizeRequest desiredSize = base.GetDesiredSize(widthConstraint, heightConstraint);
			desiredSize.Minimum = new global::Xamarin.Forms.Size(40.0, 40.0);
			return desiredSize;
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == "Content")
			{
				this.LoadContent();
				return;
			}
			if (e.PropertyName == Layout.PaddingProperty.PropertyName)
			{
				this.UpdateMargins();
				return;
			}
			if (e.PropertyName == ScrollView.OrientationProperty.PropertyName)
			{
				this.UpdateOrientation();
			}
		}
		private void UpdateOrientation()
		{
			if (base.Element.Orientation == ScrollOrientation.Horizontal)
			{
				base.Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
				return;
			}
			base.Control.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
		}
		private void UpdateMargins()
		{
			FrameworkElement frameworkElement = base.Control.Content as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			if (base.Element.Orientation == ScrollOrientation.Horizontal)
			{
				frameworkElement.Margin = new System.Windows.Thickness(base.Element.Padding.Left, 0.0, base.Element.Padding.Right, 0.0);
				return;
			}
			frameworkElement.Margin = new System.Windows.Thickness(0.0, base.Element.Padding.Top, 0.0, base.Element.Padding.Bottom);
		}
		private void LoadContent()
		{
			FrameworkElement frameworkElement = base.Control.Content as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.Margin = default(System.Windows.Thickness);
			}
			View content = base.Element.Content;
			if (content != null && content.GetRenderer() == null)
			{
				content.SetRenderer(RendererFactory.GetRenderer(content));
			}
			base.Control.Content = ((content != null) ? content.GetRenderer() : null);
			this.UpdateMargins();
		}
	}
}
