using DragonSpark.Model.Operations;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask ToOperation(this Task @this) => new ValueTask(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T> ToOperation<T>(this Task<T> @this) => new ValueTask<T>(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T> ToOperation<T>(this T @this) => new ValueTask<T>(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable<TOut> Await<TIn, TOut>(this IOperationResult<TIn, TOut> @this,
		                                                                  TIn parameter)
			=> @this.Get(parameter).ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable<TOut> Await<TFirst, TSecond, TOut>(
			this IOperationResult<(TFirst, TSecond), TOut> @this, TFirst first, TSecond second)
			=> @this.Get((first, second)).ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable Await<TFirst, TSecond>(this IOperation<(TFirst, TSecond)> @this,
		                                                                  TFirst first, TSecond second)
			=> @this.Get((first, second)).ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable<T> Await<T>(this IOperationResult<T> @this)
			=> @this.Get().ConfigureAwait(false);

		public static Task Promote(this IOperation @this) => @this.Get().AsTask();

		public static Task Promote<T>(this IOperation<T> @this, T parameter) => @this.Get(parameter).AsTask();
	}
}