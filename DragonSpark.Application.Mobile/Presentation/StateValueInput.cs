using System;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Presentation;

public readonly record struct StateValueInput<T, TValue>(T Owner, Func<TValue> Subject)
{
    public StateValueInput(T Owner, IResult<TValue> Subject) : this(Owner, Subject.Get) {}
}