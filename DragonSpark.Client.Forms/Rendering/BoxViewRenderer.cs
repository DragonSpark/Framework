using System.Windows.Shapes;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class BoxViewRenderer : ViewRenderer<BoxView, System.Windows.Shapes.Rectangle>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
		{
			base.OnElementChanged(e);
			System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
			rectangle.DataContext = base.Element;
			rectangle.SetBinding(Shape.FillProperty, new System.Windows.Data.Binding("Color")
			{
				Converter = new ColorConverter()
			});
			base.SetNativeControl(rectangle);
		}
	}
}
