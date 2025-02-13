using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Mobile.Presentation;

public readonly record struct StateInput<T, TOut>(T owner, IToken<TOut> Subject);