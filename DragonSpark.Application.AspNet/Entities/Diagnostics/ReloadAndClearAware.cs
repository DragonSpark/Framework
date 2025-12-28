using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

[UsedImplicitly]
public class ReloadAndClearAware<T> : PolicyAware<T>
{
	public ReloadAndClearAware(IStopAware<T> previous) : base(previous, ReloadAndClearPolicy.Default) {}
}

public class ReloadAndClearAware<TIn, TOut> : PolicyAware<TIn, TOut>
{
	protected ReloadAndClearAware(IStopAware<TIn, TOut> previous) : base(previous, ReloadAndClearPolicy.Default) {}
}