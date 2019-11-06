using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection
{
	public class FixedResult<TIn, TOut> : ISelect<TIn, TOut>, IActivateUsing<TOut>
	{
		readonly TOut _instance;

		public FixedResult(TOut instance) => _instance = instance;

		public TOut Get(TIn _) => _instance;
	}
}