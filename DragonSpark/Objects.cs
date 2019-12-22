using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Runtime;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T With<T>(this T @this, Action<T> action)
		{
			action(@this);
			return @this;
		}

		public static T[] Lease<T>(this ArrayPool<T> @this, int count)
		{
			var result = @this.Rent(count);
			var length = result.Length;
			for (var i = 0u; i < length; i++)
			{
				result[i] = default;
			}

			return result;
		}

		public static TOut Return<T, TOut>(this T _, TOut result) => result;

		public static TOut To<T, TOut>(this T @this, ISelect<T, TOut> select) => @this.To(select.Get);

		public static TOut To<T, TOut>(this T @this, Func<T, TOut> select) => select(@this);

		public static T To<T>(this T @this, Action<T> action)
		{
			action(@this);
			return @this;
		}

		public static T If<T>(ref this bool @this, T @true, T @false) => @this ? @true : @false;

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<Pair<TKey, TValue>> @this,
		                                                                  IEqualityComparer<TKey> comparer = null)
			=> @this.ToDictionary(x => x.Key, x => x.Value, comparer);

		public static OrderedDictionary<TKey, TValue> ToOrderedDictionary<TKey, TValue>(
			this IEnumerable<Pair<TKey, TValue>> @this,
			IEqualityComparer<TKey> comparer = null)
			=> @this.ToOrderedDictionary(x => x.Key, x => x.Value, comparer);

		public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> @this)
			=> new ReadOnlyDictionary<TKey, TValue>(@this);

		public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
			this IEnumerable<Pair<TKey, TValue>> @this)
			=> new ReadOnlyDictionary<TKey, TValue>(@this.ToDictionary());

		public static TOut AsTo<TSource, TOut>(this object target, Func<TSource, TOut> transform,
		                                       Func<TOut> resolve = null)
		{
			var @default = resolve ?? (() => default);
			var result   = target is TSource source ? transform(source) : @default();
			return result;
		}

		public static (T1, T2) Pair<T1, T2>(this T1 @this, T2 other) => ValueTuple.Create(@this, other);

		public static string NullIfEmpty(this string target) => string.IsNullOrEmpty(target) ? null : target;

		public static T Self<T>(this T @this) => @this;

		public static TOut Accept<TIn, TOut>(this TOut @this, TIn _) => @this;

		public static IEnumerable<T> Yield<T>(this T @this)
		{
			yield return @this;
		}

		public static IEnumerable<T> Yield<T>(this T @this, T other)
		{
			yield return @this;

			yield return other;
		}

		public static IDisposable ToDisposable(this object @this) => @this as IDisposable ?? EmptyDisposable.Default;

		public static T To<T>(this object @this)
			=> @this is T result
				   ? result
				   : throw new
					     InvalidOperationException($"'{@this.GetType().FullName}' is not of type {typeof(T).FullName}.");

		public static T Get<T>(this IServiceProvider @this)
		{
			if (@this is T instance)
			{
				return instance;
			}

			var service = @this.GetService(typeof(T));
			var result  = service != null ? service.To<T>() : default;
			return result;
		}
	}
}