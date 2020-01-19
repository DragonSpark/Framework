using DragonSpark.Model.Results;

namespace DragonSpark.Model.Selection
{
	public class Assume<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly IResult<ISelect<TIn, TOut>> _source;

		public Assume(IResult<ISelect<TIn, TOut>> source) => _source = source;

		public TOut Get(TIn parameter) => _source.Get().Get(parameter);
	}
}