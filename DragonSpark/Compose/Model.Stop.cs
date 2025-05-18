using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
// ReSharper disable SuspiciousTypeConversion.Global
public static partial class ExtensionMethods
{
	public static DragonSpark.Model.Operations.Results.IStopAware<T> AsStop<T>(this IResulting<T> @this)
		=> new StopAwareAdapter<T>(@this);

	public static IStopAware<TIn, TOut> AsStop<TIn, TOut>(this ISelecting<TIn, TOut> @this)
		=> new StopAdapter<TIn, TOut>(@this);

	public static OperationResultComposer<T> Ambient<T>(this OperationResultComposer<CancellationToken, T> @this)
		=> new(new StopAwareAmbientAdapter<T>(@this.Get()));

	public static IResulting<T> Ambient<T>(this ISelect<CancellationToken, ValueTask<T>> @this)
		=> new StopAwareAmbientAdapter<T>(@this);

	public static OperationResultComposer<CancellationToken, T> Bind<TIn, T>(
		this OperationResultComposer<Stop<TIn>, T> @this, TIn parameter)
		=> @this.Bind(() => parameter);

	public static OperationResultComposer<CancellationToken, T> Bind<TIn, T>(
		this OperationResultComposer<Stop<TIn>, T> @this,
		Func<TIn> parameter)
		=> new(new StopAwareBinding<TIn,T>(@this.Get(), parameter));
	/**/

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Off<TIn, TOut>(this ISelect<Stop<TIn>, ValueTask<TOut>> @this,
	                                                                Stop<TIn> parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Off<TIn, TOut>(this ISelect<Stop<TIn>, ValueTask<TOut>> @this,
	                                                                TIn parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Off<TFirst, TSecond, TOut>(
		this ISelect<Stop<(TFirst, TSecond)>, ValueTask<TOut>> @this, TFirst first, TSecond second)
		=> @this.Get(new((first, second))).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off<TFirst, TSecond>(
		this ISelect<Stop<(TFirst, TSecond)>, ValueTask> @this, TFirst first, TSecond second)
		=> @this.Get(new((first, second))).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Off<T>(this ISelect<Stop<T>, Task> @this, T parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Off(this ISelect<CancellationToken, Task> @this)
		=> @this.Get(AmbientToken.Default).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<TOut> Off<TIn, TOut>(this ISelect<Stop<TIn>, Task<TOut>> @this, TIn parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(false);


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off<T>(this ISelect<Stop<T>, ValueTask> @this, T parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off(this ISelect<Stop<None>, ValueTask> @this)
		=> @this.Get(new(None.Default)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Off<T>(this ISelect<Stop<None>, ValueTask<T>> @this)
		=> @this.Get(new(None.Default)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Off<T>(this ISelect<Stop<T>, ValueTask<T>> @this, T parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Off<T>(this ISelect<CancellationToken, ValueTask<T>> @this)
		=> @this.Get(AmbientToken.Default).ConfigureAwait(false);

	/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off<T>(this T @this) where T : ISelect<CancellationToken, ValueTask>
		=> @this.Get(AmbientToken.Default.Get()).ConfigureAwait(false);*/

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> Off<T>(this ISelect<CancellationToken, Task<T>> @this)
		=> @this.Get(AmbientToken.Default).ConfigureAwait(false);

	/**/
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> On<TIn, TOut>(this ISelect<Stop<TIn>, ValueTask<TOut>> @this,
	                                                               Stop<TIn> parameter)
		=> @this.Get(parameter).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> On<TIn, TOut>(this ISelect<Stop<TIn>, ValueTask<TOut>> @this,
	                                                               TIn parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> On<TFirst, TSecond, TOut>(
		this ISelect<Stop<(TFirst, TSecond)>, ValueTask<TOut>> @this, TFirst first, TSecond second)
		=> @this.Get(new((first, second))).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On<TFirst, TSecond>(
		this ISelect<Stop<(TFirst, TSecond)>, ValueTask> @this, TFirst first, TSecond second)
		=> @this.Get(new((first, second))).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable On<T>(this ISelect<Stop<T>, Task> @this, T parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable On(this ISelect<CancellationToken, Task> @this)
		=> @this.Get(AmbientToken.Default).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<TOut> On<TIn, TOut>(this ISelect<Stop<TIn>, Task<TOut>> @this, TIn parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On<T>(this ISelect<Stop<T>, ValueTask> @this, T parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On(this ISelect<Stop<None>, ValueTask> @this)
		=> @this.Get(new(None.Default)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this ISelect<Stop<None>, ValueTask<T>> @this)
		=> @this.Get(new(None.Default)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this ISelect<Stop<T>, ValueTask<T>> @this, T parameter)
		=> @this.Get(new(parameter)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this ISelect<CancellationToken, ValueTask<T>> @this)
		=> @this.Get(AmbientToken.Default).ConfigureAwait(true);

	/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On<T>(this T @this) where T : IResult<ValueTask>
		=> @this.Get().ConfigureAwait(true);*/

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> On<T>(this ISelect<CancellationToken, Task<T>> @this)
		=> @this.Get(AmbientToken.Default).ConfigureAwait(true);
}