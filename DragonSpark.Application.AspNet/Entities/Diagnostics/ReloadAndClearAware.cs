using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Stop;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

[UsedImplicitly]
public class ReloadAndClearAware<T> : PolicyAware<T>
{
	public ReloadAndClearAware(IStopAware<T> previous) : base(previous, ReloadAndClearPolicy.Default.Get()) {}
}

public class ReloadAndClearAware<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected ReloadAndClearAware(ISelecting<TIn, TOut> previous)
		: base(previous, ReloadAndClearPolicy.Default.Get()) {}
}