using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Application.Presentation.Controls;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ItemsControlIndexResolver : IIndexResolver
	{
		public IPagedCollectionView ResolvePageView( FrameworkElement associatedObject )
		{
			var result = associatedObject.GetParentOfType<ItemsControl>().Transform( x => x.ItemsSource.As<IPagedCollectionView>() );
			return result;
		}

		public int ResolveIndex( FrameworkElement associatedObject )
		{
			var result = associatedObject.GetParentOfType<ItemsControl>().Transform( x => x.ItemContainerGenerator.Transform( y => y.IndexFromContainer( y.ContainerFromItem( associatedObject.GetDataContext() ) ) ) );
			return result;
		}
	}
}