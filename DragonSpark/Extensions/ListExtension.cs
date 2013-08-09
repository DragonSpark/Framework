using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Extensions
{
	public static class ListExtensions
	{
		public static void Ensure<TItem>( this IList<TItem> list, TItem item )
		{
			if ( !list.Contains( item ) )
			{
				list.Add( item );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		public static void RemoveAll<T>(this IList<T> list, Func<T, bool> filter)
		{
			for ( var i = 0; i < list.Count; i++ )
			{
				if ( filter( list[ i ] ) )
				{
					list.Remove( list[ i ] );
				}
			}
		}

		public static ObservableCollection<TItem> ToObservableCollection<TItem>( this IEnumerable<TItem> target )
		{
			var result = new ObservableCollection<TItem>( target );
			return result;
		}

		public static TList AddAll<TList,TItem>( this TList target, IEnumerable<TItem> items ) where TList : class, IList<TItem>
		{
			items.Apply( target.Add );

			return target;
		}
	}
}
