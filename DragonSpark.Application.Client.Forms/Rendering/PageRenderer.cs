using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DragonSpark.Application.Client.Forms.ComponentModel;
using Page = Xamarin.Forms.Page;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class PageRenderer : VisualElementRenderer<Page, Panel>
	{
		public PageRenderer()
		{
			Background = Brushes.BurlyWood;
			// SetBinding( RendererProperties.TitleProperty, new Binding( "Element.Title" ) );
		}

		protected override void OnElementChanged( ElementChangedEventArgs<Page> e )
		{
			Tracker = new BackgroundTracker<Panel>( this, BackgroundProperty ) { Model = Element };

			base.OnElementChanged(e);
			Loaded += ( sender, args ) => Element.SendAppearing();
			Unloaded += ( sender, args ) => Element.SendDisappearing();
		}
	}
}
