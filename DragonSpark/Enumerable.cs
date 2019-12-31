using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable TooManyArguments
// ReSharper disable once PossibleInvalidOperationException
// ReSharper disable once MismatchedFileName

namespace DragonSpark
{
	public static partial class ExtensionMethods
	{
		public static T[] Open<T>(this IEnumerable<T> @this) => @this is T[] array ? array : @this.ToArray();

		public static Array<T> Result<T>(this IEnumerable<T> @this) => @this.Open();

		public static IEnumerable<T1> Introduce<T1, T2>(this IEnumerable<ISelect<T2, T1>> @this, T2 instance)
			=> @this.Introduce(instance, tuple => tuple.Item1.Get(tuple.Item2));

		public static IEnumerable<T1> Introduce<T1, T2>(this IEnumerable<Func<T2, T1>> @this, T2 instance)
			=> @this.Introduce(instance, tuple => tuple.Item1(tuple.Item2));

		public static IEnumerable<(T1, T2)> Introduce<T1, T2>(this IEnumerable<T1> @this, T2 instance)
			=> @this.Introduce(instance, x => true, Delegates<(T1, T2)>.Self);

		public static IEnumerable<T1> Introduce<T1, T2>(this IEnumerable<T1> @this, T2 instance,
		                                                Func<(T1, T2), bool> where)
			=> @this.Introduce(instance, where, tuple => tuple.Item1);

		public static IEnumerable<TOut> Introduce<T1, T2, TOut>(this IEnumerable<T1> @this, T2 instance,
		                                                        Func<(T1, T2), TOut> select)
			=> @this.Introduce(instance, x => true, select);

		public static IEnumerable<TOut> Introduce<T1, T2, TOut>(this IEnumerable<T1> @this, T2 instance,
		                                                        Func<(T1, T2), bool> where,
		                                                        Func<(T1, T2), TOut> select)
		{
			foreach (var item in @this)
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

		public static T Only<T>(this IEnumerable<T> @this) => OnlyElement<T>.Default.Get(@this);

		public static T Only<T>(this IEnumerable<T> @this, Func<T, bool> where)
			=> Compose.Start.An.Extent<OnlyElement<T>>().From(where).Get(@this);

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
			using (var enumerator = @this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					yield return enumerator.Current;
				}
			}
		}

		public static IEnumerable<T> Append<T>(this IEnumerable<T> @this, params T[] items) => @this.Concat(items);

		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, params T[] items) => items.Concat(@this);

		public static OrderedDictionary<TKey, TSource> ToOrderedDictionary<TSource, TKey>(
			this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
			=> GetOrderedDictionaryImpl(source, keySelector, x => x, null);

		public static OrderedDictionary<TKey, TElement> ToOrderedDictionary<TSource, TKey, TElement>(
			this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
			=> GetOrderedDictionaryImpl(source, keySelector, elementSelector, null);

		public static OrderedDictionary<TKey, TSource> ToOrderedDictionary<TSource, TKey>(
			this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
			=> GetOrderedDictionaryImpl(source, keySelector, x => x, comparer);

		public static OrderedDictionary<TKey, TElement> ToOrderedDictionary<TSource, TKey, TElement>(
			this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
			IEqualityComparer<TKey> comparer)
			=> GetOrderedDictionaryImpl(source, keySelector, elementSelector, comparer);

		static OrderedDictionary<TKey, TElement> GetOrderedDictionaryImpl<TSource, TKey, TElement>(
			IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
			IEqualityComparer<TKey> comparer)
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
	}
}