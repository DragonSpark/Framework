using DragonSpark.Application.Mobile.Maui.Presentation.Models;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public static class Extensions
{
    /**/
    public static IResulting<T> Using<T>(this IResulting<T> @this, IDispatcher dispatcher)
        => new DispatchAwareResulting<T>(@this, dispatcher);

    public static Task Main(this IAllocated @this) => MainThread.InvokeOnMainThreadAsync(@this.Get);
    public static Task Main<T>(this IAllocated<T> @this, T parameter)
        => MainThread.InvokeOnMainThreadAsync(() => @this.Get(parameter));
    public static Task<TOut> Main<TIn, TOut>(this IAllocating<TIn, TOut> @this, TIn parameter)
        => MainThread.InvokeOnMainThreadAsync(() => @this.Get(parameter));
}