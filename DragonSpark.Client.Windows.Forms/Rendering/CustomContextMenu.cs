using System.Windows;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public sealed class CustomContextMenu : ContextMenu
	{
		protected override DependencyObject GetContainerForItemOverride()
		{
			var item = new MenuItem(); // TODO: Don't think this works.
			item.SetBinding( HeaderedItemsControl.HeaderProperty, new System.Windows.Data.Binding("Text")
			{
				Converter = new CaseConverter()
			});
			item.Click += ( sender, args ) =>
			{
				IsOpen = false;
				item.DataContext.As<global::Xamarin.Forms.MenuItem>( x => x.Activate() );
			};
			return item;
		}
	}
}
