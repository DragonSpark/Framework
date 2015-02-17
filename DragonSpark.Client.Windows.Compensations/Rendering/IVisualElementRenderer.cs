using System;
using System.Windows;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Compensations.Rendering
{
	public interface IVisualElementRenderer : IRegisterable
	{
		event EventHandler<VisualElementChangedEventArgs> ElementChanged;
		UIElement ContainerElement
		{
			get;
		}
		VisualElement Element
		{
			get;
		}
		void SetElement(VisualElement element);
		SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint);
	}
}
