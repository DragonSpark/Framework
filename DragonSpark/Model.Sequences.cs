using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ISelect<_, T[]> Open<_, T>(this ISelect<_, IEnumerable<T>> @this) => @this.Select(x => x.Open());

		public static ISelect<_, T[]> Open<_, T>(this ISelect<_, Array<T>> @this) => @this.Select(x => x.Open());

		public static ISelect<_, Array<T>> Result<_, T>(this ISelect<_, IEnumerable<T>> @this) => @this.Query().Get();

		public static IArray<_, T> ToStore<_, T>(this ISelect<_, Array<T>> @this) => @this.ToDelegate().ToStore();

		public static IArray<_, T> ToStore<_, T>(this Func<_, Array<T>> @this) => new ArrayStore<_, T>(@this);

		public static ISequence<T> And<T>(this IResult<T> @this, params IResult<T>[] others)
			=> Sequence.Using(others.Prepend(@this).ToArray());

		public static ISequence<T> And<T>(this T[] @this, params T[] others)
			=> Sequence.From(@this.Concat(others).ToArray());

		public static ISequence<T> And<T>(this T @this, params T[] others)
			=> Sequence.From(others.Prepend(@this).ToArray());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] ToArray<T>(in this ArrayView<T> @this)
			=> @this.Length == 0 ? Empty<T>.Array : @this.Array.CopyInto(new T[@this.Length], @this.Start, @this.Length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Model.Sequences.Store<T> ToStore<T>(in this ArrayView<T> @this, IStores<T> stores)
		{
			var result = stores.Get(@this.Length);
			@this.Array.CopyInto(result.Instance, @this.Start, @this.Length);
			return result;
		}

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] CopyInto<T>(this T[] @this, T[] result, in Selection selection, in uint offset = 0)
			=> @this.CopyInto(result, selection.Start, selection.Length.IsAssigned
				                                           ? selection.Length.Instance
				                                           : (uint)result.Length - offset, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] CopyInto<T>(this T[] @this, T[] result) => @this.CopyInto(result, 0u, (uint)@this.Length);

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] CopyInto<T>(this T[] @this, T[] result, in uint start, in uint length, in uint offset = 0)
		{
			if (length < 16)
			{
				for (var i = 0; i < length; i++)
				{
					result[i + offset] = @this[start + i];
				}
			}
			else
			{
				Array.Copy(@this, start,
				           result, offset, length);
			}

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clear<T>(this T[] @this, in uint count)
		{
			if (count < 16)
			{
				for (var i = 0u; i < count; i++)
				{
					@this[i] = default;
				}
			}
			else
			{
				Array.Clear(@this, 0, (int)count);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Session<T> Session<T>(this IStorage<T> @this, in uint size)
			=> new Session<T>(@this.Get(size), @this);
	}
}