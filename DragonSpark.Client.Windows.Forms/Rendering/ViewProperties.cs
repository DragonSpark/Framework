using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public static class ViewProperties
	{
		public static readonly BindableProperty RendererProperty = BindableProperty.CreateAttached( "Renderer", typeof(IVisualElementRenderer), typeof(ViewProperties), null );
		public static IVisualElementRenderer GetRenderer(this VisualElement self)
		{
			return (IVisualElementRenderer)self.GetValue(RendererProperty);
		}
		public static void SetRenderer(this VisualElement self, IVisualElementRenderer renderer)
		{
			self.SetValue(RendererProperty, renderer);
			self.IsPlatformEnabled = renderer != null;
		}
	}
}
