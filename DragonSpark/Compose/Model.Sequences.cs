using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System.Collections.Generic;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
	public static ISelect<_, T[]> Open<_, T>(this ISelect<_, IEnumerable<T>> @this) => @this.Select(x => x.Open());

	public static ISelect<_, T[]> Open<_, T>(this ISelect<_, Array<T>> @this) => @this.Select(x => x.Open());

	public static T[] Open<T>(this IArray<T> @this) => @this.Get();

	/**/

	public static Composer<_, T[]> Open<_, T>(this Composer<_, IEnumerable<T>> @this)
		=> @this.Select(x => x.Open());

	public static Composer<_, T[]> Open<_, T>(this Composer<_, Array<T>> @this) => @this.Select(x => x.Open());

	public static Composer<_, TTo> Select<_, T, TTo>(this Composer<_, Array<T>> @this, ISelect<T[], TTo> select)
		=> @this.Open().Select(select);

	/**/

	public static T? ValueOrDefault<T>(this Option<T> @this) where T : struct
		=> @this.IsSome ? @this.Value : default(T?);

	public static T? OrDefault<T>(this Option<T> @this) => @this.IsSome ? @this.Value : default;
}