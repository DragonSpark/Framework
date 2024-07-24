using DragonSpark.Compose;

namespace DragonSpark.Model.Selection;

public sealed class VerifyInput<TIn, TOut> : Select<TIn?, TOut>
{
	public VerifyInput(ISelect<TIn, TOut> select)
		: base(Start.A.Selection<TIn?>().By.Calling<TIn>(x => x.Verify()).Select(select)) {}
}