using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Prioritize<T>( this IEnumerable<T> @this, Func<T, IAllowsPriority> determine )
		{
			var result = @this.Prioritize( x => determine( x ).Priority );
			return result;
		}

		public static IEnumerable<T> Prioritize<T>( this IEnumerable<T> @this, Func<T, Priority> determine = null )
		{
			var result = @this.OrderByDescending( determine ?? ( x => x.FromMetadata<PriorityAttribute, Priority>( y => y.Priority, () => Priority.Normal ) ) );
			return result;
		}

		public static void Each<T>( this IEnumerable<T> enumerable, Action<T> action )
		{
			AutoMapper.Internal.EnumerableExtensions.Each( enumerable, action );
		}

		public static IEnumerable<TResult> Each<T, TResult>( this IEnumerable<T> enumerable, Func<T, TResult> action )
		{
			var result = enumerable.Select( action ).ToArray();
			return result;
		}

		public static IEnumerable<TItem> Append<TItem>( this IEnumerable<TItem> target, params TItem[] items )
		{
			var result = target.Concat( items );
			return result;
		}

		public static IEnumerable<TItem> AsItem<TItem>( this TItem target )
		{
			return Append( target, null );
		}

		public static IEnumerable<TItem> Append<TItem>( this TItem target, IEnumerable<TItem> second )
		{
			var first =  (TItem[])target.Transform( x => new[] { x }, Enumerable.Empty<TItem> );
			var result = second != null ? first.Concat( second ) : first;
			return result;
		}

		public static IEnumerable<Tuple<TFirst, TLast>> TupleWith<TFirst, TLast>( this IEnumerable<TFirst> target, IEnumerable<TLast> other )
		{
			var first = target.ToList();
			var result = first.Select( x => new Tuple<TFirst, TLast>( x, other.ElementAtOrDefault( first.IndexOf( x ) ) ) ).ToArray();
			return result;
		}

		public static IEnumerable<TItem> NotNull<TItem>( this IEnumerable<TItem> target )
		{
			var result = target.Where( x => !Equals( x, default( TItem ) ) );
			return result;
		}

		public static T FirstOrDefaultOfType<T>(this IEnumerable enumerable)
		{
			return enumerable.OfType<T>().FirstOrDefault();
		}
	}
}