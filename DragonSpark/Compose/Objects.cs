﻿using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Runtime;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static T With<T>(this T @this, Action<T> action)
		{
			action(@this);
			return @this;
		}

		public static (T1, T2) With<T1, T2>(this (T1, T2) @this, Action<T1, T2> action)
		{
			action(@this.Item1, @this.Item2);
			return @this;
		}

		public static T[] Lease<T>(this ArrayPool<T> @this, int count)
		{
			var result = @this.Rent(count);
			var length = result.Length;
			for (var i = 0u; i < length; i++)
			{
				result[i] = default!;
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
		                                                                  IEqualityComparer<TKey>? comparer = null)
			where TKey : notnull
			=> @this.ToDictionary(x => x.Key, x => x.Value, comparer);

		public static OrderedDictionary<TKey, TValue> ToOrderedDictionary<TKey, TValue>(
			this IEnumerable<Pair<TKey, TValue>> @this,
			IEqualityComparer<TKey>? comparer = default)
			where TKey : notnull
			=> @this.ToOrderedDictionary(x => x.Key, x => x.Value, comparer);

		public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> @this)
			where TKey : notnull
			=> new ReadOnlyDictionary<TKey, TValue>(@this);

		public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
			this IEnumerable<Pair<TKey, TValue>> @this)
			where TKey : notnull
			=> new ReadOnlyDictionary<TKey, TValue>(@this.ToDictionary());

		public static TOut AsTo<TSource, TOut>(this object target, Func<TSource, TOut> transform,
		                                       Func<TOut>? resolve = default)
		{
			var @default = resolve ?? (() => default!);
			var result   = target is TSource source ? transform(source) : @default();
			return result;
		}

		public static (T1, T2) Tuple<T1, T2>(this T1 @this, T2 other) => (@this, other);


		public static Pair<T1, T2> Pair<T1, T2>(this T1 @this, T2 other) => Pairs.Create(@this, other);


		public static string? NullIfEmpty(this string? target) => string.IsNullOrEmpty(target) ? null : target;

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

		public static ValueTask Disposed(this IDisposable @this)
		{
			// ReSharper disable once SuspiciousTypeConversion.Global
			if (@this is IAsyncDisposable disposable)
			{
				return disposable.DisposeAsync();
			}

			@this.Dispose();

			return Task.CompletedTask.ToOperation();
		}

		public static IDisposable ToDisposable(this object @this) => @this as IDisposable ?? EmptyDisposable.Default;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Verify<T>(this T? @this, string message = "Provided instance is not assigned.") where T : class
			=> @this ?? throw new InvalidOperationException(message);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? Account<T>(this T? @this) where T : class => @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
			return result!;
		}

		public static bool Inverse(this in bool @this) => !@this;

		public static int Degrade(this in ulong @this) => (int)@this;

		public static int Degrade(this in uint @this) => (int)@this;

		public static uint Grade(this in int @this) => (uint)@this;

		public static ulong Grade(this in long @this) => (ulong)@this;

		public static uint Integer(this in ulong @this) => (uint)@this;
		
		public static byte Byte(this in int @this) => (byte)@this;

		public static byte Byte(this in uint @this) => (byte)@this;

		public static uint Next(this in uint @this) => @this + 1;

		public static byte Plus(this in byte @this, in byte other) => (byte)(@this + other);
		public static byte Minus(this in byte @this, in byte other) => (byte)(@this - other);

		public static byte Next(this in byte @this) => @this.Plus(1);
		public static byte Previous(this in byte @this) => @this.Minus(1);
		public static uint Previous(this in uint @this) => @this - 1;
		public static int Previous(this in int @this) => @this - 1;

		public static int Next(this in int @this) => @this + 1;

		/**/

		public static DirectoryInfo Directory(this DirectoryInfo @this, string path)
			=> new DirectoryInfo(Path.Combine(@this.FullName, path));
	}
}