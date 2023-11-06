using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Diagnostics;

public class ReloadAware<T> : PolicyAwareOperation<T>
{
	public ReloadAware(IOperation<T> previous) : base(previous, ConcurrencyAwarePolicy.Default.Get()) {}
}

public class ReloadAware<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected ReloadAware(ISelecting<TIn, TOut> previous) : base(previous, ConcurrencyAwarePolicy.Default.Get()) {}
}