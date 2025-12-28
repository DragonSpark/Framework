using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public class RapidReloadAware<T> : PolicyAware<T>
{
	protected RapidReloadAware(IStopAware<T> previous) : base(previous, RapidReloadPolicy.Default) {}
}

[UsedImplicitly]
public class RapidReloadAware<TIn, TOut> : PolicyAware<TIn, TOut>
{
	protected RapidReloadAware(IStopAware<TIn, TOut> previous) : base(previous, RapidReloadPolicy.Default) {}
}