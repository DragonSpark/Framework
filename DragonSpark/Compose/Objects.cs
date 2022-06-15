using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut Return<T, TOut>(this T _, TOut result) => result;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut To<T, TOut>(this T @this, ISelect<T, TOut> select) => @this.To(select.Get);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TOut To<T, TOut>(this T @this, Func<T, TOut> select) => select(@this);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

	public static IDisposable ToDisposable(this object @this) => @this as IDisposable ?? EmptyDisposable.Default;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Verify<T>(this T? @this, string message = "Provided instance is not assigned.")
		=> @this ?? throw new InvalidOperationException(message);


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Value<T>(this T? @this, string message = "Provided instance is not assigned.")
		where T : struct
		=> @this ?? throw new InvalidOperationException(message);


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? Account<T>(this T @this) => (T?)@this;

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

	public static ITime Then(this DateTimeOffset @this) => new FixedTime(@this);

	public static bool Inverse(this in bool @this) => !@this;

	public static short Degrade(this in ushort @this) => (short)@this;
	public static int Degrade(this in ulong @this) => (int)@this;

	public static int Degrade(this in uint @this) => (int)@this;

	public static uint Grade(this in int @this) => (uint)@this;

	public static ulong Grade(this in long @this) => (ulong)@this;

	public static int Contract(this in long @this) => (int)@this;
	public static short Contract(this in int @this) => (short)@this;
	public static byte Contract(this in short @this) => (byte)@this;

	public static uint Contract(this in ulong @this) => (uint)@this;
	public static ushort Contract(this in uint @this) => (ushort)@this;
	public static byte Contract(this in ushort @this) => (byte)@this;

	public static ushort Expand(this in byte @this) => @this;
	public static uint Expand(this in ushort @this) => @this;
	public static ulong Expand(this in uint @this) => @this;

	public static short Expand(this in sbyte @this) => @this;
	public static int Expand(this in short @this) => @this;
	public static long Expand(this in int @this) => @this;

	public static float Real(this in byte @this) => @this;
	public static float Real(this in ushort @this) => @this;
	public static float Real(this in uint @this) => @this;

	public static float Real(this in sbyte @this) => @this;
	public static float Real(this in short @this) => @this;
	public static float Real(this in int @this) => @this;


	public static double Expand(this in float @this) => @this;

	public static decimal Expand(this in double @this) => (decimal)@this;

	public static double Contract(this in decimal @this) => (double)@this;
	public static float Contract(this in double @this) => (float)@this;
	public static long Clip(this in float @this) => (long)@this;
	public static long Clip(this in double @this) => (long)@this;

	public static long Clip(this in decimal @this) => (long)@this;

	public static uint Next(this in uint @this) => @this + 1;

	public static byte Plus(this in byte @this, in byte other) => (byte)(@this + other);
	public static byte Minus(this in byte @this, in byte other) => (byte)(@this - other);

	public static byte Next(this in byte @this) => @this.Plus(1);

	public static byte Previous(this in byte @this) => @this.Minus(1);
	public static uint Previous(this in uint @this) => @this - 1;
	public static int Previous(this in int @this) => @this - 1;

	public static int Next(this in int @this) => @this + 1;

	/**/

	public static DirectoryInfo Subdirectory(this DirectoryInfo @this, string path)
		=> new DirectoryInfo(Path.Combine(@this.FullName, path));

	public static FileInfo File(this DirectoryInfo @this, string path)
		=> new FileInfo(Path.Combine(@this.FullName, path));
}