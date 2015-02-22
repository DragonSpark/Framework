using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public static class RendererFactory
	{
		public static IVisualElementRenderer GetRenderer(VisualElement view)
		{
			var result = Registrar.Registered.GetHandler<IVisualElementRenderer>(view.GetType()) ?? new ViewRenderer();
			result.SetElement(view);
			return result;
		}
	}
}
