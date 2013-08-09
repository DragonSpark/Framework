using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "It's more of a view than a collection." )]
    public class ViewModelCollectionView<TSource,TViewModel> : ViewCollection<TViewModel> where TViewModel : class
    {
        readonly IDictionary<TSource,TViewModel> cache = new Dictionary<TSource, TViewModel>();
        readonly ICollectionView collectionView;
        readonly Func<TSource,TViewModel> factory;

        public ViewModelCollectionView( ICollectionView collectionView, Func<TSource,TViewModel> factory )
        {
            this.collectionView = collectionView;
            this.factory = factory;
            collectionView.CollectionChanged += ( sender, args ) => RefreshItems();
        }

        public void RefreshItems()
        {
            var items = collectionView.SourceCollection.OfType<TSource>().Select( x => cache.Ensure( x, factory ) ).ToArray();
            this.Reset( items );
        }
    }
}