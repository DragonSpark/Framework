using DragonSpark.Application.Mobile.Maui.Presentation.Models;
using DragonSpark.Model.Operations.Results;
using Microsoft.Maui.Dispatching;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public static class Extensions
{
    /**/
    public static IResulting<T> Using<T>(this IResulting<T> @this, IDispatcher dispatcher)
        => new DispatchAwareResulting<T>(@this, dispatcher);
}
