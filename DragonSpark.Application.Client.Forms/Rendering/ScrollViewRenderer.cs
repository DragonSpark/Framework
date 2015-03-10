using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class ScrollViewRenderer : ViewRenderer<ScrollView, ScrollViewer>
	{
		Animatable animatable;

		public ScrollViewRenderer()
		{
			base.AutoPackage = false;
		}

		protected IScrollViewController Controller
		{
			get { return Element; }
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null)
			{
				((IScrollViewController)e.OldElement).ScrollToRequested -= OnScrollToRequested;
			}
			if (e.NewElement != null)
			{
				if (base.Control == null)
				{
					base.SetNativeControl(new ScrollViewer
					{
						// ManipulationMode = ManipulationMode.Control
					});
					base.Control.LayoutUpdated += ( sender, args ) => UpdateScrollPosition();
				}
				((IScrollViewController)e.NewElement).ScrollToRequested += OnScrollToRequested;
			}
			SetNativeControl( new ScrollViewer() );
			SizeChanged += ( sender, args ) =>
			{
				Control.Width = ActualWidth;
				Control.Height = ActualHeight;
			};
			UpdateOrientation();
			LoadContent();
		}

		void OnScrollToRequested(object sender, ScrollToRequestedEventArgs e)
		{
			if (this.animatable == null && e.ShouldAnimate)
			{
				this.animatable = new Animatable();
			}
			ScrollToPosition arg_34_0 = e.Position;
			double x = e.ScrollX;
			double y = e.ScrollY;
			if (e.Mode == ScrollToMode.Element)
			{
				Xamarin.Forms.Point scrollPositionForElement = this.Controller.GetScrollPositionForElement(e.Element as VisualElement, e.Position);
				x = scrollPositionForElement.X;
				y = scrollPositionForElement.Y;
			}
			if (base.Control.VerticalOffset == y && base.Control.HorizontalOffset == x)
			{
				return;
			}
			if (e.ShouldAnimate)
			{
				Animation animation = new Animation(delegate(double v)
				{
					this.UpdateScrollOffset(ScrollViewRenderer.GetDistance(this.Control.ViewportWidth, x, v), ScrollViewRenderer.GetDistance(this.Control.ViewportHeight, y, v));
				}, 0.0, 1.0, null, null);
				animation.Commit(this.animatable, "ScrollTo", 16u, 500u, Easing.CubicInOut, delegate(double v, bool d)
				{
					this.UpdateScrollOffset(x, y);
					this.Controller.SendScrollFinished();
				}, null);
				return;
			}
			this.UpdateScrollOffset(x, y);
			this.Controller.SendScrollFinished();
		}
		private void UpdateScrollOffset(double x, double y)
		{
			if (base.Element.Orientation == ScrollOrientation.Horizontal)
			{
				base.Control.ScrollToHorizontalOffset(x);
				return;
			}
			base.Control.ScrollToVerticalOffset(y);
		}
		private static double GetDistance(double start, double position, double v)
		{
			return start + (position - start) * v;
		}
		private void UpdateScrollPosition()
		{
			if (base.Element != null)
			{
				this.Controller.SetScrolledPosition(base.Control.HorizontalOffset, base.Control.VerticalOffset);
			}
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
