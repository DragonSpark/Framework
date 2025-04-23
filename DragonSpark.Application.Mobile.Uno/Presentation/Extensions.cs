using DragonSpark.Application.Mobile.Uno.Presentation.Models;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Results;
using Uno.Extensions;
using Uno.Extensions.Reactive;

namespace DragonSpark.Application.Mobile.Uno.Presentation;

public static class Extensions
{
	public static IState<TOut> AsState<T, TOut>(this IResulting<TOut> @this, T owner)
		where T : class where TOut : notnull
		=> @this.AsToken().AsState(owner);

	public static IState<TOut> AsState<T, TOut>(this IToken<TOut> @this, T owner) where T : class where TOut : notnull
		=> State<T, TOut>.Default.Get(new(owner, @this));

	public static IState<TOut> AsState<T, TOut>(this IResult<TOut> @this, T owner) where T : class where TOut : notnull
		=> StateValue<T, TOut>.Default.Get(new(owner, @this));

    public static IState<bool> AsStateAssigned<T, TOut>(this IResult<TOut?> @this, T owner) where T : class
        => StateValue<T, bool>.Default.Get(new(owner, @this.Then().Select(x => x is not null)));

	public static IFeed<T> AsFeed<T>(this IResulting<T> @this) where T : notnull => @this.AsToken().AsFeed();

	public static IFeed<T> AsFeed<T>(this IToken<T> @this) where T : notnull => Feed<T>.Default.Get(@this);

    /**/
    public static IResulting<T> Using<T>(this IResulting<T> @this, IDispatcher dispatcher)
        => new DispatchAwareResulting<T>(@this, dispatcher);
}