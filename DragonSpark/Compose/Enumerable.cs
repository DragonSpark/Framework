using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Runtime;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable TooManyArguments
// ReSharper disable once MismatchedFileName

namespace DragonSpark.Compose;

public static partial class ExtensionMethods
{
	public static T[] Open<T>(this IEnumerable<T> @this) => @this is T[] array ? array : @this.ToArray();

	public static Array<T> Result<T>(this IEnumerable<T> @this) => @this.Open();

	public static IArray<T> Promote<T>(this Array<T> @this) => new Instances<T>(@this);

	public static IEnumerable<T1> Introduce<T1, T2>(this IEnumerable<Func<T2, T1>> @this, T2 instance)
		=> @this.Introduce(instance, tuple => tuple.Item1(tuple.Item2));

	public static IEnumerable<(T1, T2)> Introduce<T1, T2>(this IEnumerable<T1> @this, T2 instance)
		=> @this.Introduce(instance, _ => true, Delegates<(T1, T2)>.Self);

	public static IEnumerable<T1> Introduce<T1, T2>(this IEnumerable<T1> @this, T2 instance,
	                                                Func<(T1, T2), bool> where)
		=> @this.Introduce(instance, where, tuple => tuple.Item1);

	public static IEnumerable<TOut> Introduce<T1, T2, TOut>(this IEnumerable<T1> @this, T2 instance,
	                                                        Func<(T1, T2), TOut> select)
		=> @this.Introduce(instance, _ => true, select);

	public static IEnumerable<TOut> Introduce<T1, T2, TOut>(this IEnumerable<T1> @this, T2 instance,
	                                                        Func<(T1, T2), bool> where,
	                                                        Func<(T1, T2), TOut> select)
	{
		foreach (var item in @this.AsValueEnumerable())
		{
			var tuple = (item, instance);
			if (where(tuple))
			{
				yield return select(tuple);
			}
		}
	}

	public static bool AnyTrue(this IEnumerable<bool> source)
	{
		foreach (var b in source)
		{
			if (b)
			{
				return true;
			}
		}

		return false;
	}

	public static bool AnyFalse(this IEnumerable<bool> source)
	{
		foreach (var b in source)
		{
			if (!b)
			{
				return true;
			}
		}

		return false;
	}

	public static bool All(this IEnumerable<bool> source)
	{
		foreach (var b in source)
		{
			if (!b)
			{
				return false;
			}
		}

		return true;
	}

	public static T? Only<T>(this IEnumerable<T> @this)
		=> DragonSpark.Model.Sequences.Query.Only<T>.Default.Get(@this).Account();

	public static TOut? Only<T, TOut>(this IEnumerable<T> @this, Func<T, TOut> select)
	{
		var only   = @this.Only();
		var result = only is not null ? select(only) : default;
		return result;
	}

	public static void ForEach<TIn, TOut>(this IEnumerable<TIn> @this, Func<TIn, TOut> select)
	{
		foreach (var @in in @this)
		{
			select(@in);
		}
	}

	public static void ForEach<T>(this IEnumerable<T> @this, Action<T> select)
	{
		foreach (var @in in @this)
		{
			select(@in);
		}
	}

	public static IEnumerable<T> Hide<T>(this IEnumerable<T> @this)
	{
		using var enumerator = @this.GetEnumerator();
		while (enumerator.MoveNext())
		{
			yield return enumerator.Current;
		}
	}

	public static IEnumerable<T> Append<T>(this IEnumerable<T> @this, params T[] items) => @this.Concat(items);

	public static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, params T[] items) => items.Concat(@this);

	public static OrderedDictionary<TKey, TSource> ToOrderedDictionary<TSource, TKey>(
		this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		where TKey : notnull
		=> GetOrderedDictionaryImpl(source, keySelector, x => x, null);

	public static OrderedDictionary<TKey, TElement> ToOrderedDictionary<TSource, TKey, TElement>(
		this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		where TKey : notnull
		=> GetOrderedDictionaryImpl(source, keySelector, elementSelector, null);

	public static OrderedDictionary<TKey, TSource> ToOrderedDictionary<TSource, TKey>(
		this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		where TKey : notnull
		=> GetOrderedDictionaryImpl(source, keySelector, x => x, comparer);

	public static OrderedDictionary<TKey, TElement> ToOrderedDictionary<TSource, TKey, TElement>(
		this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
		IEqualityComparer<TKey>? comparer)
		where TKey : notnull
		=> GetOrderedDictionaryImpl(source, keySelector, elementSelector, comparer);

	static OrderedDictionary<TKey, TElement> GetOrderedDictionaryImpl<TSource, TKey, TElement>(
		IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
		IEqualityComparer<TKey>? comparer)
		where TKey : notnull
	{
		var result = comparer == null
			             ? new OrderedDictionary<TKey, TElement>()
			             : new OrderedDictionary<TKey, TElement>(comparer);

		foreach (var sourceItem in source)
		{
			result.Add(keySelector(sourceItem), elementSelector(sourceItem));
		}

		return result;
	}

	public static IEnumerable<TLeft> Outstanding<TLeft, TRight, TKey>(this IEnumerable<TLeft> @this,
	                                                                  IEnumerable<TRight> other,
	                                                                  Func<TLeft, TKey> leftKey,
	                                                                  Func<TRight, TKey> rightKey)
		=> @this.OuterJoin(other, leftKey, rightKey, (left, right) => right is null ? left : default)
		        .Where(x => x is not null)!;

	// ATTRIBUTION: https://stackoverflow.com/a/39020068/10340424
	public static IEnumerable<TResult> OuterJoin<TLeft, TRight, TKey, TResult>(this IEnumerable<TLeft> @this,
	                                                                           IEnumerable<TRight> other,
	                                                                           Func<TLeft, TKey> leftKey,
	                                                                           Func<TRight, TKey> rightKey,
	                                                                           Func<TLeft, TRight?, TResult> result)
		=> @this.GroupJoin(other, leftKey, rightKey, (l, r) => new { l, r })
		        .SelectMany(o => o.r.DefaultIfEmpty(), (l, r) => new { lft = l.l, rght = r })
		        .Select(o => result(o.lft, o.rght));
}