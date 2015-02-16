using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;
using Frame = Xamarin.Forms.Frame;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class FrameRenderer : ViewRenderer<Frame, Border>
	{
		public FrameRenderer()
		{
			base.AutoPackage = false;
		}
		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);
			base.SetNativeControl(new Border());
			this.PackChild();
			this.UpdateBorder();
		}
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == "Content")
			{
				this.PackChild();
				return;
			}
			if (e.PropertyName == global::Xamarin.Forms.Frame.OutlineColorProperty.PropertyName || e.PropertyName == global::Xamarin.Forms.Frame.HasShadowProperty.PropertyName)
			{
				this.UpdateBorder();
			}
		}
		private void UpdateBorder()
		{
			base.Control.CornerRadius = new CornerRadius(5.0);
			if (base.Element.OutlineColor != Color.Default)
			{
				base.Control.BorderBrush = base.Element.OutlineColor.ToBrush();
				base.Control.BorderThickness = new System.Windows.Thickness(1.0);
				return;
			}
			base.Control.BorderBrush = new Color(0.0, 0.0, 0.0, 0.0).ToBrush();
		}
		private void PackChild()
		{
			if (base.Element.Content == null)
			{
				return;
			}
			if (base.Element.Content.GetRenderer() == null)
			{
				base.Element.Content.SetRenderer(RendererFactory.GetRenderer(base.Element.Content));
			}
			UIElement child = base.Element.Content.GetRenderer() as UIElement;
			base.Control.Child = child;
		}
	}
}
