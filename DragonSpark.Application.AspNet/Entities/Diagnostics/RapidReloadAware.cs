using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Stop;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public class RapidReloadAware<T> : PolicyAware<T>
{
	protected RapidReloadAware(IStopAware<T> previous) : base(previous, RapidReloadPolicy.Default.Get()) {}
}

[UsedImplicitly]
public class RapidReloadAware<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected RapidReloadAware(ISelecting<TIn, TOut> previous) : base(previous, RapidReloadPolicy.Default.Get()) {}
}