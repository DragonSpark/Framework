using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static class ListExtensions
    {
        public static TList Reset<TList,TItem>( this TList target, IEnumerable<TItem> source, NotifyCollectionChangedEventHandler handler = null ) where TList : IObservableCollection<TItem>
        {
            handler.NotNull( x => target.CollectionChanged -= x );
			
            var remove = target.Except( source ).ToArray();
            remove.Apply( y => target.Remove( y ) );

            var add = source.Except( target ).ToArray();
            add.Apply( target.Add );
            handler.NotNull( x => target.CollectionChanged += x );
            return target;
        }
    }
}