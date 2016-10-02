using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;

namespace DragonSpark.Extensions
{
	public static class EnumerableExtensions
	{
		public static T[] Fixed<T>( this IEnumerable<T> @this )
		{
			var array = @this is ImmutableArray<T> ? ((ImmutableArray<T>)@this).ToArray() : @this as T[] ?? @this.ToArray();
			var result = array.Length > 0 ? array : (T[])Enumerable.Empty<T>();
			return result;
		}

		public static IEnumerable<ValueTuple<T1, T2>> Introduce<T1, T2>( this ImmutableArray<T1> @this, T2 instance ) => Introduce( @this, instance, Where<ValueTuple<T1, T2>>.Always, Delegates<ValueTuple<T1, T2>>.Self );

		public static IEnumerable<T1> Introduce<T1, T2>( this IEnumerable<Func<T2, T1>> @this, T2 instance ) => Introduce( @this, instance, tuple => tuple.Item1( tuple.Item2 ) );

		public static IEnumerable<T1> Introduce<T1, T2>( this ImmutableArray<Func<T2, T1>> @this, T2 instance ) => Introduce( @this, instance, tuple => tuple.Item1( tuple.Item2 ) );

		public static IEnumerable<T1> Introduce<T1, T2>( this ImmutableArray<T1> @this, T2 instance, Func<ValueTuple<T1, T2>, bool> where ) => Introduce( @this, instance, @where, tuple => tuple.Item1 );

		public static IEnumerable<TResult> Introduce<T1, T2, TResult>( this ImmutableArray<T1> @this, T2 instance, Func<ValueTuple<T1, T2>, TResult> select ) => Introduce( @this, instance, Where<ValueTuple<T1, T2>>.Always, @select );

		public static IEnumerable<TResult> Introduce<T1, T2, TResult>( this ImmutableArray<T1> @this, T2 instance, Func<ValueTuple<T1, T2>, bool> where, Func<ValueTuple<T1, T2>, TResult> select )
		{
			foreach ( var item in @this )
			{
				var tuple = ValueTuple.Create( item, instance );
				if ( where( tuple ) )
				{
					yield return select( tuple );
				}
			}
		}

		public static IEnumerable<ValueTuple<T1, T2>> Introduce<T1, T2>( this IEnumerable<T1> @this, T2 instance ) => Introduce( @this, instance, Where<ValueTuple<T1, T2>>.Always, Delegates<ValueTuple<T1, T2>>.Self );

		public static IEnumerable<T1> Introduce<T1, T2>( this IEnumerable<T1> @this, T2 instance, Func<ValueTuple<T1, T2>, bool> where ) => Introduce( @this, instance, @where, tuple => tuple.Item1 );

		public static IEnumerable<TResult> Introduce<T1, T2, TResult>( this IEnumerable<T1> @this, T2 instance, Func<ValueTuple<T1, T2>, TResult> select ) => Introduce( @this, instance, Where<ValueTuple<T1, T2>>.Always, @select );

		public static IEnumerable<TResult> Introduce<T1, T2, TResult>( this IEnumerable<T1> @this, T2 instance, Func<ValueTuple<T1, T2>, bool> where, Func<ValueTuple<T1, T2>, TResult> select )
		{
			foreach ( var item in @this )
			{
				var tuple = ValueTuple.Create( item, instance );
				if ( where( tuple ) )
				{
					yield return select( tuple );
				}
			}
		}

		public static bool All( this IEnumerable<bool> source )
		{
			foreach ( var b in source )
			{
				if ( !b )
				{
					return false;
				}
			}

			return true;
		}

		public static IEnumerable<T> Prioritize<T>( this IEnumerable<T> @this ) => Prioritize( @this, Support<T>.PriorityLocator );

		public static IEnumerable<T> Prioritize<T>( this IEnumerable<T> @this, Func<T, IPriorityAware> locator ) => @this.OrderBy( locator, PriorityComparer.Default );

		public static TTo WithFirst<TFrom, TTo>( this IEnumerable<TFrom> @this, Func<TFrom, TTo> with, Func<TTo> defaultFunction = null ) => WithFirst( @this, Where<TFrom>.Always, with, defaultFunction );

		public static TTo WithFirst<TFrom, TTo>( this IEnumerable<TFrom> @this, Func<TFrom, bool> where, Func<TFrom, TTo> with, Func<TTo> defaultFunction = null ) => @this.WhereAssigned().FirstOrDefault( @where ).With( with, defaultFunction );

		public static T Only<T>( this IEnumerable<T> @this ) => Only( @this, Where<T>.Always );
		public static T Only<T>( this ImmutableArray<T> @this, Func<T, bool> where ) => @this.AsEnumerable().Only( where );
		public static T Only<T>( this IEnumerable<T> @this, Func<T, bool> where )
		{
			var enumerable = @this.Where( where ).ToArray();
			var result = enumerable.Length == 1 ? enumerable[0] : default(T);
			return result;
		}

		public static void Each<T>( this ImmutableArray<T> @this, Action<T> action ) => @this.AsEnumerable().ForEach( action );
		public static void Each<T>( this IEnumerable<T> @this, Action<T> action ) => @this.ForEach( action );

		public static IEnumerable<TResult> Each<T, TResult>( this IEnumerable<T> enumerable, Func<T, TResult> action ) => enumerable.Select( action ).ToArray();

		public static IEnumerable<T> Yield<T>( this T @this ) => EnumerableEx.Return( @this );

		public static TItem[] ToItem<TItem>( this TItem target ) where TItem : class => Array<TItem>.Default.Get( target );
		class Array<T> : Cache<T, T[]> where T : class
		{
			public static Array<T> Default { get; } = new Array<T>();
			Array() : base( arg => new[] { arg } ) {}
		}

		public static IEnumerable<T> Append<T>( this T @this, params T[] second ) => @this.Append( second.AsEnumerable() );
		public static IEnumerable<T> Append<T>( this T @this, IEnumerable<T> second ) => @this.Append_( second );

		static IEnumerable<T> Append_<T>( this T @this, IEnumerable<T> second )
		{
			yield return @this;
			foreach ( var element1 in second )
				yield return element1;
		}

		public static T[] Fixed<T>( this IEnumerable<T> @this, params T[] items ) => @this.Append( items ).Fixed();

		public static IEnumerable<T> Append<T>( this IEnumerable<T> @this, params T[] items ) => @this.Concat( items );

		public static IEnumerable<T> Append<T>( this IEnumerable<T> @this, [Optional]T element )
		{
			foreach ( var element1 in @this )
				yield return element1;
			yield return element;
		}

		public static IEnumerable<T> Prepend<T>( this T @this, params T[] second ) => @this.Prepend( second.AsEnumerable() );
		public static IEnumerable<T> Prepend<T>( this T @this, IEnumerable<T> second ) => @this.Prepend_( second );

		static IEnumerable<T> Prepend_<T>( this T @this, IEnumerable<T> second )
		{
			foreach ( var item in second )
				yield return item;
			yield return @this;
		}

		public static IEnumerable<T> Prepend<T>( this IEnumerable<T> @this, params T[] items ) => items.Concat( @this );

		public static IEnumerable<ValueTuple<T1, T2>> Tuple<T1, T2>( this IEnumerable<T1> target, IEnumerable<T2> other ) => target.Zip( other, ValueTuple.Create ).ToArray();

		public static T FirstAssigned<T>( this IEnumerable<T> @this ) => @this.WhereAssigned().FirstOrDefault();

		// public static TTo FirstAssigned<TFrom, TTo>( this ImmutableArray<TFrom> @this, Func<TFrom, TTo> projection ) => @this.ToArray().FirstAssigned( projection );

		public static TTo FirstAssigned<TFrom, TTo>( this IEnumerable<TFrom> @this, Func<TFrom, TTo> projection ) => @this.WhereAssigned().Select( projection ).FirstAssigned();

		public static IEnumerable<T> WhereAssigned<T>( this ImmutableArray<T> target ) => target.AsEnumerable().WhereAssigned();
		public static IEnumerable<T> WhereAssigned<T>( this IEnumerable<T> target )
		{
			foreach ( var item in target )
			{
				if ( Where<T>.Assigned( item ) )
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<TResult> SelectAssigned<TSource, TResult>( this ImmutableArray<TSource> @this, Func<TSource, TResult> select ) => @this.AsEnumerable().SelectAssigned( select );
		public static IEnumerable<TResult> SelectAssigned<TSource, TResult>( this IEnumerable<TSource> @this, Func<TSource, TResult> select ) => @this.Select( select ).WhereAssigned();

		public static T FirstOrDefaultOfType<T>( this IEnumerable enumerable ) => enumerable.OfType<T>().FirstOrDefault();

		public static T PeekOrDefault<T>( this System.Collections.Generic.Stack<T> @this ) => @this.Any() ? @this.Peek() : default(T);

		static class Support<T>
		{
			public static Func<T, IPriorityAware> PriorityLocator { get; } = PriorityAwareLocator<T>.Default.ToSourceDelegate();
		}
	}
}