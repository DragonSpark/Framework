using System.ComponentModel;
using System.Windows;

namespace DragonSpark.Application.Presentation.Interaction
{
	public interface IIndexResolver
	{
		IPagedCollectionView ResolvePageView( FrameworkElement associatedObject );

		int ResolveIndex( FrameworkElement associatedObject );
	}
}