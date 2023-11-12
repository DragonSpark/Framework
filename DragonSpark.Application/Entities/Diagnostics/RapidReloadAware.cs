using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Diagnostics;

public class RapidReloadAware<T> : PolicyAwareOperation<T>
{
	protected RapidReloadAware(IOperation<T> previous) : base(previous, RapidReloadPolicy.Default.Get()) {}
}

public class RapidReloadAware<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected RapidReloadAware(ISelecting<TIn, TOut> previous) : base(previous, RapidReloadPolicy.Default.Get()) {}
}