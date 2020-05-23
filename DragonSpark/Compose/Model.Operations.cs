using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	// ReSharper disable SuspiciousTypeConversion.Global
	public static partial class ExtensionMethods
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask ToOperation(this Task @this) => new ValueTask(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T> ToOperation<T>(this Task<T> @this) => new ValueTask<T>(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T> ToOperation<T>(this T @this) => new ValueTask<T>(@this);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable<TOut> Await<TIn, TOut>(this ISelect<TIn, ValueTask<TOut>> @this,
		                                                                  TIn parameter)
			=> @this.Token().Get(parameter).ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable<TOut> Await<TFirst, TSecond, TOut>(
			this ISelect<(TFirst, TSecond), ValueTask<TOut>> @this, TFirst first, TSecond second)
			=> @this.Token().Get((first, second)).ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable Await<TFirst, TSecond>(
			this ISelect<(TFirst, TSecond), ValueTask> @this, TFirst first, TSecond second)
			=> @this.Token().Get((first, second)).ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable Await<T>(this ISelect<T, ValueTask> @this, T parameter)
			=> @this.Token().Get(parameter).ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredValueTaskAwaitable<T> Await<T>(this IResult<ValueTask<T>> @this)
			=> @this.Token().Get().ConfigureAwait(false);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ISelect<T, ValueTask> Token<T>(this ISelect<T, ValueTask> @this)
			=> @this is IToken token ? token.Get().Return(@this) : @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ISelect<TIn, ValueTask<TOut>> Token<TIn, TOut>(this ISelect<TIn, ValueTask<TOut>> @this)
			
			=> @this is IToken token ? token.Get().Return(@this) : @this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IResult<ValueTask<T>> Token<T>(this IResult<ValueTask<T>> @this)
			=> @this is IToken token ? token.Get().Return(@this) : @this;

		public static Task Promote(this IOperation @this) => @this.Get().AsTask();

		public static Task Promote<T>(this IOperation<T> @this, T parameter) => @this.Get(parameter).AsTask();
	}
}