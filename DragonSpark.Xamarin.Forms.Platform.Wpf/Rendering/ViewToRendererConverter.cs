using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class ViewToRendererConverter : System.Windows.Data.IValueConverter
	{
		private class WrapperControl : ContentControl
		{
			private readonly View view;
			public WrapperControl(View view)
			{
				this.view = view;
				view.SetRenderer(RendererFactory.GetRenderer(view));
				base.Content = view.GetRenderer().ContainerElement;
				FrameworkElement frameworkElement = view.GetRenderer().ContainerElement as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.Loaded += delegate(object sender, RoutedEventArgs args)
					{
						base.InvalidateMeasure();
					};
				}
			}
			protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
			{
				this.view.Layout(new Rectangle(0.0, 0.0, finalSize.Width, finalSize.Height));
				FrameworkElement frameworkElement = base.Content as FrameworkElement;
				frameworkElement.Arrange(new Rect(0.0, 0.0, finalSize.Width, finalSize.Height));
				return finalSize;
			}
			protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
			{
				FrameworkElement frameworkElement = base.Content as FrameworkElement;
				frameworkElement.Measure(availableSize);
				global::Xamarin.Forms.Size request = this.view.GetSizeRequest(availableSize.Width, availableSize.Height).Request;
				return new System.Windows.Size(request.Width, request.Height);
			}
		}
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			View view = value as View;
			if (view == null)
			{
				return null;
			}
			return new ViewToRendererConverter.WrapperControl(view);
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
