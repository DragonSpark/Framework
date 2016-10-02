using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class ImmutableArrayExtensions
	{
		public static IEnumerable<T> AsEnumerable<T>( this ImmutableArray<T> source ) => source.ToArray();

		public static IEnumerable<T> Union<T>( this ImmutableArray<T> first, IEnumerable<T> second ) => first.ToArray().Union( second );

		public static IEnumerable<T> Except<T>( this IEnumerable<T> first, ImmutableArray<T> second ) => first.Except( second.AsEnumerable() );

		public static IEnumerable<T> Except<T>( this ImmutableArray<T> first, IEnumerable<T> second ) => first.ToArray().Except( second );

		public static IEnumerable<T> Concat<T>( this ImmutableArray<T> first, IEnumerable<T> second ) => first.ToArray().Concat( second );

		public static IEnumerable<T> Concat<T>( this IEnumerable<ImmutableArray<T>> sources ) => sources.Select( array => array.ToArray() ).Concat();
	}
}