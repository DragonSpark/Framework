using DragonSpark.Compose;

namespace DragonSpark.Model.Selection;

sealed class Verified<_, T> : Select<_, T> where T : class
{
	public Verified(ISelect<_, T?> select) : base(select.Select(x => x.Verify())) {}
}

// TODO
public sealed class VerifyInput<TIn, TOut> : Select<TIn?, TOut>
{
	public VerifyInput(ISelect<TIn, TOut> select)
		: base(Start.A.Selection<TIn?>().By.Calling<TIn>(x => x.Verify()).Select(select)) {}
}