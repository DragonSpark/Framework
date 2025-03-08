using DragonSpark.Compose.Model.Operations;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
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
	public static OperationResultComposer<TIn, TOut> Or<TIn, TOut>(this OperationResultComposer<TIn, TOut?> @this,
	                                                               ISelecting<TIn, TOut> second)
		where TOut : class => @this.Or(second.Off);

	public static OperationResultComposer<TIn, TOut> Or<TIn, TOut>(this OperationResultComposer<TIn, TOut?> @this,
	                                                               DragonSpark.Model.Operations.Await<TIn, TOut> next)
		where TOut : class
		=> new DragonSpark.Model.Operations.Selection.Coalesce<TIn, TOut>(@this, next).Then();

	public static OperationResultComposer<TIn, TOut?> OrMaybe<TIn, TOut>(this OperationResultComposer<TIn, TOut?> @this,
	                                                                     ISelecting<TIn, TOut?> second)
		where TOut : class => @this.OrMaybe(second.Off);

	public static OperationResultComposer<TIn, TOut?> OrMaybe<TIn, TOut>(this OperationResultComposer<TIn, TOut?> @this,
	                                                                     DragonSpark.Model.Operations.Await<TIn, TOut?> next)
		where TOut : class
		=> new DragonSpark.Model.Operations.Selection.Maybe<TIn, TOut>(@this, next).Then();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValueTask ToOperation(this Task @this) => new(@this);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValueTask<T> ToOperation<T>(this Task<T> @this) => new(@this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask<T> ToOperation<T>(this T @this) => new(@this);

    /**/

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> Off<T>(this Task<T> @this) => @this.ConfigureAwait(false);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Off<T>(this ValueTask<T> @this) => @this.ConfigureAwait(false);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Off(this Task @this) => @this.ConfigureAwait(false);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off(this ValueTask @this) => @this.ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Off<TIn, TOut>(this ISelect<TIn, ValueTask<TOut>> @this,
	                                                                  TIn parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> Off<TFirst, TSecond, TOut>(
		this ISelect<(TFirst, TSecond), ValueTask<TOut>> @this, TFirst first, TSecond second)
		=> @this.Get((first, second)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off<TFirst, TSecond>(
		this ISelect<(TFirst, TSecond), ValueTask> @this, TFirst first, TSecond second)
		=> @this.Get((first, second)).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Off<T>(this ISelect<T, Task> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable Off(this IAllocated @this) => @this.Get().ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<TOut> Off<TIn, TOut>(this ISelect<TIn, Task<TOut>> @this, TIn parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off<T>(this ISelect<T, ValueTask> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off(this ISelect<None, ValueTask> @this)
		=> @this.Get(None.Default).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Off<T>(this ISelect<None, ValueTask<T>> @this)
		=> @this.Get(None.Default).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Off<T>(this IAltering<T> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> Off<T>(this IResult<ValueTask<T>> @this)
		=> @this.Get().ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable Off<T>(this T @this) where T : IResult<ValueTask>
		=> @this.Get().ConfigureAwait(false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> Off<T>(this IResult<Task<T>> @this)
		=> @this.Get().ConfigureAwait(false);

    /**/
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> On<T>(this Task<T> @this) => @this.ConfigureAwait(true);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this ValueTask<T> @this) => @this.ConfigureAwait(true);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable On(this Task @this) => @this.ConfigureAwait(true);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On(this ValueTask @this) => @this.ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> On<TIn, TOut>(this ISelect<TIn, ValueTask<TOut>> @this,
                                                                   TIn parameter)
		=> @this.Get(parameter).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<TOut> On<TFirst, TSecond, TOut>(
		this ISelect<(TFirst, TSecond), ValueTask<TOut>> @this, TFirst first, TSecond second)
		=> @this.Get((first, second)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On<TFirst, TSecond>(
		this ISelect<(TFirst, TSecond), ValueTask> @this, TFirst first, TSecond second)
		=> @this.Get((first, second)).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable On<T>(this ISelect<T, Task> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable On(this IAllocated @this) => @this.Get().ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<TOut> On<TIn, TOut>(this ISelect<TIn, Task<TOut>> @this, TIn parameter)
		=> @this.Get(parameter).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On<T>(this ISelect<T, ValueTask> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On(this ISelect<None, ValueTask> @this)
		=> @this.Get(None.Default).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this ISelect<None, ValueTask<T>> @this)
		=> @this.Get(None.Default).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this IAltering<T> @this, T parameter)
		=> @this.Get(parameter).ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable<T> On<T>(this IResult<ValueTask<T>> @this)
		=> @this.Get().ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredValueTaskAwaitable On<T>(this T @this) where T : IResult<ValueTask>
		=> @this.Get().ConfigureAwait(true);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ConfiguredTaskAwaitable<T> On<T>(this IResult<Task<T>> @this) => @this.Get().ConfigureAwait(true);
    /**/

	public static Task Allocate(this IOperation<None> @this) => @this.Get().AsTask();

	public static Task Allocate(this IResult<ValueTask> @this) => @this.Get().AsTask();
	public static Task<T> Allocate<T>(this IResult<ValueTask<T>> @this) => @this.Get().AsTask();

	public static Task Allocate(this Func<ValueTask> @this) => @this().AsTask();

	public static Task Allocate<T>(this ISelect<T, ValueTask> @this, T parameter) => @this.Get(parameter).AsTask();

	public static Task<TOut> Allocate<TIn, TOut>(this ISelect<TIn, ValueTask<TOut>> @this, TIn parameter)
		=> @this.Get(parameter).AsTask();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<TOut?> Accounting<TIn, TOut>(this ISelecting<TIn, TOut> @this, TIn parameter)
		=> await @this.Off(parameter);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<Token<TOut?>> Accounting<TIn, TOut>(this ISelectingToken<TIn, TOut> @this,
	                                                                  Token<TIn> parameter)
	{
		var (subject, token) = await @this.Off(parameter);
		return new(subject.Account(), token);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<T?> Accounting<T>(this IResulting<T> @this) => await @this.Off();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<TOut> Verifying<TIn, TOut>(this ISelecting<TIn, TOut?> @this, TIn parameter)
		=> (await @this.Off(parameter)).Verify();

	public static ISelecting<TIn, TOut> Verifying<TIn, TOut>(this ISelecting<TIn, TOut?> @this)
		=> new Verifying<TIn, TOut>(@this);

	public static OperationResultComposer<TIn, TOut> Verifying<TIn, TOut>(
		this OperationResultComposer<TIn, TOut?> @this)
		=> new Verifying<TIn, TOut>(@this.Get()).Then();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async ValueTask<T> Verifying<T>(this IResulting<T?> @this) => (await @this.Off()).Verify();
}