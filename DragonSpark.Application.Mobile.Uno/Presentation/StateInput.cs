using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Mobile.Uno.Presentation;

public readonly record struct StateInput<T, TOut>(T Owner, IToken<TOut> Subject);