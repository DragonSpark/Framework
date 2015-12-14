using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class EnumerableExtensions
	{
		public static T[] Fixed<T>( this IEnumerable<T> @this )
		{
			var result = @this as T[] ?? @this.ToArray();
			return result;
		}

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

		public static T Only<T>( this IEnumerable<T> @this )
		{
			var enumerable = @this as T[] ?? @this.ToArray();
			var result = enumerable.Count() == 1 ? enumerable.Single() : default(T);
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

		public static IEnumerable<TItem> ToItem<TItem>( this TItem target )
		{
			return new[] { target };
		}

		public static IEnumerable<TItem> Append<TItem>( this TItem target, IEnumerable<TItem> second )
		{
			var items = second ?? Enumerable.Empty<TItem>();
			var result = target.ToItem().Concat( items  );
			return result;
		}

		public static IEnumerable<TItem> Prepend<TItem>( this TItem target, IEnumerable<TItem> second )
		{
			var items = second ?? Enumerable.Empty<TItem>();
			var result = items.Concat( target.ToItem() );
			return result;
		}

		public static IEnumerable<TItem> Append<TItem>( this IEnumerable<TItem> target, params TItem[] items )
		{
			var result = target.Concat( items );
			return result;
		}

		public static IEnumerable<TItem> Prepend<TItem>( this IEnumerable<TItem> target, params TItem[] items )
		{
			var result = items.Concat( target );
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