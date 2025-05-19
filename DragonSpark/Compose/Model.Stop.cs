using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Selection;
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
	public static Stop<T> Stop<T>(this T @this) => new(@this);

	public static Stop<T> Stop<T>(this T @this, CancellationToken stop) => new(@this, stop);

	/* Results: */
	public static OperationResultComposer<T> Alternate<T>(this OperationResultComposer<CancellationToken, T> @this)
		=> new(new StopAwareAmbientAdapter<T>(@this.Get()));

	public static IResulting<T> Alternate<T>(this ISelect<CancellationToken, ValueTask<T>> @this)
		=> new StopAwareAmbientAdapter<T>(@this);

	public static DragonSpark.Model.Operations.Results.IStopAware<T> AsStop<T>(this IResulting<T> @this)
		=> new DragonSpark.Model.Operations.Results.StopAwareAdapter<T>(@this);

	/*Operation*/

	public static IOperation<T> Alternate<T>(this Composer<Stop<T>, ValueTask> @this)
		=> @this.Get().Alternate();

	public static IOperation<T> Alternate<T>(this ISelect<Stop<T>, ValueTask> @this)
		=> new StopAwareOperationAdapter<T>(@this);

	public static DragonSpark.Model.Operations.IStopAware<T> AsStop<T>(this Composer<T, ValueTask> @this)
		=> new DragonSpark.Model.Operations.StopAwareAdapter<T>(@this.Get());

	public static DragonSpark.Model.Operations.IStopAware<T> AsStop<T>(this ISelect<T, ValueTask> @this)
		=> new DragonSpark.Model.Operations.StopAwareAdapter<T>(@this);

	public static OperationComposer<Stop<T>> Terminate<T, TOut>(this OperationResultComposer<Stop<T>, TOut> @this,
	                                                      ISelect<Stop<TOut>, ValueTask> command)
		=> @this.Terminate(command.Get);

	public static OperationComposer<Stop<T>> Terminate<T, TOut>(this OperationResultComposer<Stop<T>, TOut> @this,
	                                                      Func<Stop<TOut>, ValueTask> command)
		=> new(new Terminate<T, TOut>(@this.Get(), command));

	/* SELECTING */

	public static ISelecting<TIn, TOut> Alternate<TIn, TOut>(this IStopAware<TIn, TOut> @this)
		=> new SelectingAdapter<TIn, TOut>(@this);

	public static IStopAware<TIn, TOut> AsStop<TIn, TOut>(this ISelecting<TIn, TOut> @this)
		=> new StopAdapter<TIn, TOut>(@this);

	public static OperationResultComposer<CancellationToken, T> Bind<TIn, T>(
		this OperationResultComposer<Stop<TIn>, T> @this, TIn parameter)
		=> @this.Bind(() => parameter);

	public static OperationResultComposer<CancellationToken, T> Bind<TIn, T>(
		this OperationResultComposer<Stop<TIn>, T> @this,
		Func<TIn> parameter)
		=> new(new StopAwareBinding<TIn, T>(@this.Get(), parameter));

	public static OperationResultComposer<Stop<TIn>, TTo> Select<TIn, TOut, TTo>(
		this OperationResultComposer<Stop<TIn>, TOut> @this, ISelect<Stop<TOut>, ValueTask<TTo>> select)
		=> @this.Select<TIn, TOut, TTo>(select.Get);

	public static OperationResultComposer<Stop<TIn>, TTo> Select<TIn, TOut, TTo>(
		this OperationResultComposer<Stop<TIn>, TOut> @this,
		Func<Stop<TOut>, ValueTask<TTo>> select)
		=> new(new StopAware<TIn, TOut, TTo>(@this.Get().Get, select));

	/**/

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Get<T>(this ISelect<Stop<None>, T> @this) => @this.Get(AmbientToken.Default); // TODO: Audit

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Get<T>(this ISelect<Stop<None>, T> @this, CancellationToken stop)
		=> @this.Get(new(None.Default, stop));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this ISelect<Stop<None>, ValueTask<T>> @this,
	                                                    CancellationToken stop)
		=> @this.Get(new(None.Default, stop)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Off<TOut>(this ISelect<Stop<None>, ValueTask<TOut>> @this,
	                                                           CancellationToken stop)
		=> @this.Get(new(None.Default, stop)).ConfigureAwait(false);

	/**/

	/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Off<TIn, TOut>(this ISelect<Stop<TIn>, ValueTask<TOut>> @this,
	                                                                Stop<TIn> parameter)
		=> @this.Get(parameter).ConfigureAwait(false);*/

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