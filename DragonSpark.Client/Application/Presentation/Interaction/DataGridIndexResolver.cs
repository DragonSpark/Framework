using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class DataGridIndexResolver : IIndexResolver
	{
		public IPagedCollectionView ResolvePageView( FrameworkElement associatedObject )
		{
			var result = associatedObject.GetParentOfType<DataGrid>().Transform( x => x.ItemsSource.As<IPagedCollectionView>() );
			return result;
		}

		public int ResolveIndex( FrameworkElement associatedObject )
		{
			var result = associatedObject.GetParentOfType<DataGridRow>().GetIndex();
			return result;
		}
	}
}