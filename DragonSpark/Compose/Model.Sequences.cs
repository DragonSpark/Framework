using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Array = System.Array;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ISelect<_, T[]> Open<_, T>(this ISelect<_, IEnumerable<T>> @this) => @this.Select(x => x.Open());

		public static ISelect<_, T[]> Open<_, T>(this ISelect<_, Array<T>> @this) => @this.Select(x => x.Open());

		/**/

		public static Model.Selector<_, T[]> Open<_, T>(this Model.Selector<_, IEnumerable<T>> @this)
			=> @this.Select(x => x.Open());

		public static Model.Selector<_, T[]> Open<_, T>(this Model.Selector<_, Array<T>> @this) => @this.Select(x => x.Open());

		public static Model.Selector<_, TTo> Select<_, T, TTo>(this Model.Selector<_, Array<T>> @this, ISelect<T[], TTo> select)
			=> @this.Open().Select(select);

		/**/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] ToArray<T>(in this ArrayView<T> @this)
			=> @this.Length == 0
				   ? Empty<T>.Array
				   : @this.Array.CopyInto(new T[@this.Length], @this.Start, @this.Length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Store<T> ToStore<T>(in this ArrayView<T> @this, IStores<T> stores)
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
					@this[i] = default!;
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

		public static async ValueTask<ICollection<T>> Emit<TIn, T>(this IEnumerable<TIn> @this, Await<TIn, T> select)
		{
			var result = new List<T>();
			foreach (var @in in @this.AsValueEnumerable())
			{
				result.Add(await select(@in!));
			}
			return result;
		}

		public static async IAsyncEnumerable<T> Adapt<T>(this IEnumerable<ValueTask<T>> @this)
		{
			using var iterator = @this.GetEnumerator();
			while (iterator.MoveNext())
			{
				yield return await iterator.Current;
			}
		}

		public static async IAsyncEnumerable<T> Adapt<T>(this IEnumerable<ConfiguredValueTaskAwaitable<T>> @this)
		{
			using var iterator = @this.GetEnumerator();
			while (iterator.MoveNext())
			{
				yield return await iterator.Current;
			}
		}

		public static async IAsyncEnumerable<TTo> Select<T, TTo>(this IAsyncEnumerable<T> @this, Await<T, TTo> select)
		{
			var iterator = @this.GetAsyncEnumerator();
			while (await iterator.MoveNextAsync())
			{
				yield return await select(iterator.Current);
			}
		}

	}
}