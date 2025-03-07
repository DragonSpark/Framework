using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using Uno.Extensions.Reactive;

namespace DragonSpark.Application.Mobile.Presentation;

sealed class Feed<TOut> : ISelect<IToken<TOut>, IFeed<TOut>> where TOut : notnull
{
    public static Feed<TOut> Default { get; } = new();

    Feed() {}

    public IFeed<TOut> Get(IToken<TOut> parameter) => Feed.Async(parameter.Get);
}