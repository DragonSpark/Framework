using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using Uno.Extensions.Reactive;

namespace DragonSpark.Application.Mobile.Presentation;

sealed class State<T, TOut> : ISelect<StateInput<T, TOut>, IState<TOut>> where T : class where TOut : notnull
{
	public static State<T, TOut> Default { get; } = new();

	State() {}

	public IState<TOut> Get(StateInput<T, TOut> parameter)
	{
		var (owner, subject) = parameter;
        return State.Async<T, TOut>(owner, subject.Get);
	}
}

sealed class Feed<TOut> : ISelect<IToken<TOut>, IFeed<TOut>> where TOut : notnull
{
	public static Feed<TOut> Default { get; } = new();

	Feed() {}

	public IFeed<TOut> Get(IToken<TOut> parameter) => Feed.Async(parameter.Get);
}