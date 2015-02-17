using System;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public class VisualElementPackager
	{
		private readonly Panel panel;
		private readonly IVisualElementRenderer renderer;
		private bool loaded;
		public VisualElementPackager(IVisualElementRenderer renderer)
		{
			if (renderer == null)
			{
				throw new ArgumentNullException("renderer");
			}
			this.panel = (renderer.ContainerElement as Panel);
			if (this.panel == null)
			{
				throw new ArgumentException("Renderer's container element must be a Panel or Panel subclass");
			}
			this.renderer = renderer;
		}
		public void Load()
		{
			if (this.loaded)
			{
				return;
			}
			this.loaded = true;
			this.renderer.Element.ChildAdded += new EventHandler<ElementEventArgs>(this.HandleChildAdded);
			this.renderer.Element.ChildRemoved += new EventHandler<ElementEventArgs>(this.HandleChildRemoved);
			foreach (Element current in this.renderer.Element.LogicalChildren)
			{
				this.HandleChildAdded(this.renderer.Element, new ElementEventArgs(current));
			}
		}
		private void HandleChildRemoved(object sender, ElementEventArgs e)
		{
			VisualElement visualElement = e.Element as VisualElement;
			if (visualElement == null)
			{
				return;
			}
			UIElement uIElement = visualElement.GetRenderer() as UIElement;
			if (uIElement != null)
			{
				this.panel.Children.Remove(uIElement);
			}
		}
		private void HandleChildAdded(object sender, ElementEventArgs e)
		{
			VisualElement visualElement = e.Element as VisualElement;
			if (visualElement == null)
			{
				return;
			}
			IVisualElementRenderer visualElementRenderer;
			visualElement.SetRenderer(visualElementRenderer = RendererFactory.GetRenderer(visualElement));
			this.panel.Children.Add(visualElementRenderer.ContainerElement);
		}
	}
}
