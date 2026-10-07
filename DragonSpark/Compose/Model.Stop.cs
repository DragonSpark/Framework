using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Runtime.CompilerServices;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
// ReSharper disable SuspiciousTypeConversion.Global
public static partial class ExtensionMethods
{
    public static Stop<T> Stop<T>(this T @this) => @this.Stop(CancellationToken.None);

    public static Stop<T> Stop<T>(this T @this, CancellationToken stop) => new(@this, stop);

    /* Results: */

    public static DragonSpark.Model.Operations.Results.Stop.IStopAware<T> AsStop<T>(this IResulting<T> @this)
        => new DragonSpark.Model.Operations.Results.Stop.StopAwareAdapter<T>(@this);

    /*Operation*/

    public static DragonSpark.Model.Operations.Stop.IStopAware<T> AsStop<T>(this Composer<T, ValueTask> @this)
        => new DragonSpark.Model.Operations.Stop.StopAwareAdapter<T>(@this.Get());

    public static DragonSpark.Model.Operations.Stop.IStopAware<T> AsStop<T>(this ISelect<T, ValueTask> @this)
        => new DragonSpark.Model.Operations.Stop.StopAwareAdapter<T>(@this);

    public static IStopAware AsStop(this ResultComposer<ValueTask> @this) => new StopAwareAdapter(@this.Get());

    public static IStopAware AsStop(this IResult<ValueTask> @this) => new StopAwareAdapter(@this);

    public static OperationComposer<Stop<T>> Terminate<T, TOut>(this OperationResultComposer<Stop<T>, TOut> @this,
                                                                ISelect<Stop<TOut>, ValueTask> command)
        => @this.Terminate(command.Get);

    public static OperationComposer<Stop<T>> Terminate<T, TOut>(this OperationResultComposer<Stop<T>, TOut> @this,
                                                                Func<Stop<TOut>, ValueTask> command)
        => new(new Terminate<T, TOut>(@this.Get(), command));

    public static OperationComposer<CancellationToken> Bind<T>(this OperationComposer<Stop<T>> @this, T parameter)
        => @this.Bind(() => parameter);

    public static OperationComposer<CancellationToken> Bind<T>(this OperationComposer<Stop<T>> @this, Func<T> parameter)
        => new(new StopAwareBinding<T>(@this.Get(), parameter));

    public static OperationComposer<T> Bind<T>(this OperationComposer<Stop<T>> @this, CancellationToken parameter)
        => new(new ParameterBinding<T>(@this.Get(), parameter));

    public static TaskComposer<T> Bind<T>(this TaskComposer<Stop<T>> @this, CancellationToken parameter)
        => new(new DragonSpark.Model.Operations.Allocated.Stop.ParameterBinding<T>(@this.Get(), parameter));

    /* SELECTING */

    public static IStopAware<TIn, TOut> AsStop<TIn, TOut>(this ISelecting<TIn, TOut> @this)
        => new StopAdapter<TIn, TOut>(@this);

    public static OperationResultComposer<CancellationToken, T> Bind<TIn, T>(
        this OperationResultComposer<Stop<TIn>, T> @this, TIn parameter)
        => @this.Bind(() => parameter);

    public static OperationResultComposer<CancellationToken, T> Bind<TIn, T>(
        this OperationResultComposer<Stop<TIn>, T> @this, Func<TIn> parameter)
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
    public static T Get<T>(this ISelect<Stop<None>, T> @this, CancellationToken stop)
        => @this.Get(new(None.Default, stop));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConfiguredValueTaskAwaitable<T> On<T>(this ISelect<Stop<None>, ValueTask<T>> @this,
                                                        CancellationToken stop)
        => @this.Get(new(None.Default, stop)).ConfigureAwait(true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConfiguredValueTaskAwaitable<TOut> Off<TOut>(this ISelect<Stop<None>, ValueTask<TOut>> @this,
                                                               CancellationToken stop)
        => @this.Get(new(None.Default, stop)).ConfigureAwait(false);

    /**/

    public static SelectedLogOperationExceptionComposer<T, TOther> Use<T, TOther>(this OperationComposer<Stop<T>> @this,
                                                                                  ILogException<TOther> log)
	    => new(@this.Out(), log);
    public static PolicyAwareLogOperationExceptionComposer<T> UsePolicy<T>(this OperationComposer<Stop<T>> @this, 
                                                                           ILogException<T> log)
	    => new(@this.Get().Out(), log);
}