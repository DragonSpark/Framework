using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Results
{
	public class FixedSelectedSingleton<TIn, TOut> : DeferredSingleton<TOut>
	{
		public FixedSelectedSingleton(ISelect<TIn, TOut> select, TIn parameter)
			: base(new FixedSelection<TIn, TOut>(select, parameter).Get) {}
	}
}