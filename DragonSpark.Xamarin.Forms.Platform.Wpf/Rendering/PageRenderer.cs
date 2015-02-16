using System.Windows.Controls;
using Page = Xamarin.Forms.Page;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	public class PageRenderer : VisualElementRenderer<Page, Panel>
	{
		protected override void OnElementChanged( ElementChangedEventArgs<Page> e )
		{
			Tracker = new BackgroundTracker<Panel>( this, BackgroundProperty ) { Model = Element };

			base.OnElementChanged(e);
			Loaded += ( sender, args ) => Element.SendAppearing();
			Unloaded += ( sender, args ) => Element.SendDisappearing();
		}
	}
}
