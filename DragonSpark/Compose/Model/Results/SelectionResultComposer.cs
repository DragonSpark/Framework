using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Results;

public sealed class SelectionResultComposer<TIn, TOut> : ResultComposer<ISelect<TIn, TOut>>
{
	public SelectionResultComposer(IResult<ISelect<TIn, TOut>> instance) : base(instance) {}

	public Composer<TIn, TOut> Assume() => new Assume<TIn, TOut>(Get()).Then();
}