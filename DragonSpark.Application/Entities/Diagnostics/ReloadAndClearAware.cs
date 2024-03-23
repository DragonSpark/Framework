using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Application.Entities.Diagnostics;

[UsedImplicitly]
public class ReloadAndClearAware<T> : PolicyAwareOperation<T>
{
	public ReloadAndClearAware(IOperation<T> previous) : base(previous, ReloadAndClearPolicy.Default.Get()) {}
}

public class ReloadAndClearAware<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected ReloadAndClearAware(ISelecting<TIn, TOut> previous)
		: base(previous, ReloadAndClearPolicy.Default.Get()) {}
}