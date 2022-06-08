using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
// ReSharper disable SuspiciousTypeConversion.Global
public static partial class ExtensionMethods
{
	public static OperationResultSelector<TIn, TOut> Or<TIn, TOut>(this OperationResultSelector<TIn, TOut?> @this,
	                                                               ISelecting<TIn, TOut> second)
		where TOut : class => @this.Or(second.Await);

	public static OperationResultSelector<TIn, TOut> Or<TIn, TOut>(this OperationResultSelector<TIn, TOut?> @this,
	                                                               Await<TIn, TOut> next)
		where TOut : class
		=> new DragonSpark.Model.Operations.Coalesce<TIn, TOut>(@this, next).Then();

	public static OperationResultSelector<TIn, TOut?> OrMaybe<TIn, TOut>(this OperationResultSelector<TIn, TOut?> @this,
	                                                                     ISelecting<TIn, TOut?> second)
		where TOut : class => @this.OrMaybe(second.Await);

	public static OperationResultSelector<TIn, TOut?> OrMaybe<TIn, TOut>(this OperationResultSelector<TIn, TOut?> @this,
	                                                                     Await<TIn, TOut?> next)
		where TOut : class
		=> new DragonSpark.Model.Operations.Maybe<TIn, TOut>(@this, next).Then();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValueTask ToOperation(this Task @this) => new(@this);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValueTask<T> ToOperation<T>(this Task<T> @this) => new(@this);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValueTask<T> ToOperation<T>(this T @this) => new(@this);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Await<TIn, TOut>(this ISelect<TIn, ValueTask<TOut>> @this,
	                                                                  TIn parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Await<TFirst, TSecond, TOut>(
		this ISelect<(TFirst, TSecond), ValueTask<TOut>> @this, TFirst first, TSecond second)
		=> @this.Get((first, second)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Await<TFirst, TSecond>(
		this ISelect<(TFirst, TSecond), ValueTask> @this, TFirst first, TSecond second)
		=> @this.Get((first, second)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Await<T>(this ISelect<T, Task> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Await(this IAllocated @this) => @this.Get().ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<TOut> Await<TIn, TOut>(this ISelect<TIn, Task<TOut>> @this, TIn parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Await<T>(this ISelect<T, ValueTask> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Await(this ISelect<None, ValueTask> @this)
		=> @this.Get(None.Default).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Await<T>(this ISelect<None, ValueTask<T>> @this)
		=> @this.Get(None.Default).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Await<T>(this IAltering<T> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Await<T>(this IResult<ValueTask<T>> @this)
		=> @this.Get().ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Await<T>(this T @this) where T : IResult<ValueTask>
		=> @this.Get().ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> Await<T>(this IResult<Task<T>> @this)
		=> @this.Get().ConfigureAwait(false);

	public static Task Allocate(this IOperation<None> @this) => @this.Get().AsTask();

	public static Task Allocate(this IResult<ValueTask> @this) => @this.Get().AsTask();

	public static Task Allocate(this Func<ValueTask> @this) => @this().AsTask();

	public static Task Allocate<T>(this ISelect<T, ValueTask> @this, T parameter) => @this.Get(parameter).AsTask();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<TOut?> Accounting<TIn, TOut>(this ISelecting<TIn, TOut> @this, TIn parameter)
		=> await @this.Await(parameter);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<T?> Accounting<T>(this IResulting<T> @this) => await @this.Await();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<TOut> Verifying<TIn, TOut>(this ISelecting<TIn, TOut?> @this, TIn parameter)
		=> (await @this.Await(parameter)).Verify();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<T> Verifying<T>(this IResulting<T?> @this) => (await @this.Await()).Verify();
}