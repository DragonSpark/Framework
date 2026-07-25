using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Selection;

public sealed class Verifying<TIn, TOut> : Selecting<TIn, TOut>
{
	public Verifying(ISelect<TIn, ValueTask<TOut?>> select) : base(@select.Then().Select(x => x.Verify())) {}
}