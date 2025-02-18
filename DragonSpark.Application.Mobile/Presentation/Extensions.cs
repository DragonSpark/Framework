using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Uno.Extensions.Reactive;

namespace DragonSpark.Application.Mobile.Presentation;

public static class Extensions
{
	public static IState<TOut> AsState<T, TOut>(this IResulting<TOut> @this, T owner)
		where T : class where TOut : notnull
		=> @this.AsToken().AsState(owner);

	public static IState<TOut> AsState<T, TOut>(this IToken<TOut> @this, T owner) where T : class where TOut : notnull
		=> State<T, TOut>.Default.Get(new(owner, @this));

	public static IFeed<T> AsFeed<T>(this IResulting<T> @this) where T : notnull => @this.AsToken().AsFeed();

	public static IFeed<T> AsFeed<T>(this IToken<T> @this) where T : notnull => Feed<T>.Default.Get(@this);
}