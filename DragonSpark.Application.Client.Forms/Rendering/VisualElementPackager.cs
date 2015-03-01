using System;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
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
			this.renderer.Element.ChildrenReordered += new EventHandler(this.HandleChildrenReordered);
			
			foreach (Element current in this.renderer.Element.LogicalChildren)
			{
				this.HandleChildAdded(this.renderer.Element, new ElementEventArgs(current));
			}
		}

		private void HandleChildrenReordered(object sender, EventArgs e)
		{
			this.EnsureZIndex();
		}
		private void EnsureZIndex()
		{
			for (int i = 0; i < this.renderer.Element.LogicalChildren.Count; i++)
			{
				VisualElement self = (VisualElement)this.renderer.Element.LogicalChildren[i];
				IVisualElementRenderer visualElementRenderer = self.GetRenderer();
				if (visualElementRenderer != null)
				{
					Canvas.SetZIndex(visualElementRenderer.ContainerElement, i + 1);
				}
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
			this.EnsureZIndex();
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
			this.EnsureZIndex();
		}
	}
}
