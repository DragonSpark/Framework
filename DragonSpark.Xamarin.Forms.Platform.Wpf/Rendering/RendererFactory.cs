using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public static class RendererFactory
	{
		public static IVisualElementRenderer GetRenderer(VisualElement view)
		{
			IVisualElementRenderer visualElementRenderer = Registrar.Registered.GetHandler<IVisualElementRenderer>(view.GetType()) ?? new ViewRenderer();
			visualElementRenderer.SetElement(view);
			return visualElementRenderer;
		}
	}
}
