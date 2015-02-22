using System.Windows.Controls;
using System.Windows.Data;
using DragonSpark.Application.Forms.ComponentModel;
using Page = Xamarin.Forms.Page;

namespace DragonSpark.Application.Forms.Rendering
{
	public class PageRenderer : VisualElementRenderer<Page, Panel>
	{
		public PageRenderer()
		{
			SetBinding( ShellProperties.TitleProperty, new Binding( "Element.Title" ) );
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
