using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model {
	public sealed class SelectionResultContext<TIn, TOut> : ResultContext<ISelect<TIn, TOut>>
	{
		public SelectionResultContext(IResult<ISelect<TIn, TOut>> instance) : base(instance) {}

		public Selector<TIn, TOut> Assume() => new Assume<TIn, TOut>(Get()).Then();
	}
}