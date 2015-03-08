using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static class NotifyCollectionChangedEventArgsExtensions
    {
        public static IEnumerable<TItem> AllItems<TItem>( this NotifyCollectionChangedEventArgs target )
        {
            var result = target.NewItems.Transform( x => Enumerable.OfType<TItem>( x ), Enumerable.Empty<TItem> ).Union( target.OldItems.Transform( x => x.OfType<TItem>(), Enumerable.Empty<TItem> ) );
            return result;
        }
    }
}