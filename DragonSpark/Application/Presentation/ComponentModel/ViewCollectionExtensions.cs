using System.Collections.Generic;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public static class ViewCollectionExtensions
    {
        public static IEnumerable<TResult> Adding<TResult>( this IObservableCollection<TResult> target, IEnumerable<TResult> items )
        {
            target.AddRange(items);
            return target;
        }
    }
}