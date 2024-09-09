using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Results;

public sealed class SelectionDelegateResultComposer<TIn, TOut> : ResultComposer<Func<TIn, TOut>>
{
	public SelectionDelegateResultComposer(IResult<Func<TIn, TOut>> instance) : base(instance) {}

	public Composer<TIn, TOut> Assume() => new DelegatedAssume<TIn, TOut>(Get().Get).Then();
}