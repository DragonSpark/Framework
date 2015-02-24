using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class VisualElementChangedEventArgs : ElementChangedEventArgs<VisualElement>
	{
		public VisualElementChangedEventArgs(VisualElement oldElement, VisualElement newElement) : base(oldElement, newElement)
		{
		}
	}
}
