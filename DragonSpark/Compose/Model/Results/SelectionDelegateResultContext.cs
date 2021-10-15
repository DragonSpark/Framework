using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Results;

public sealed class SelectionDelegateResultContext<TIn, TOut> : ResultContext<Func<TIn, TOut>>
{
	public SelectionDelegateResultContext(IResult<Func<TIn, TOut>> instance) : base(instance) {}

	public Selector<TIn, TOut> Assume() => new DelegatedAssume<TIn, TOut>(Get().Get).Then();
}