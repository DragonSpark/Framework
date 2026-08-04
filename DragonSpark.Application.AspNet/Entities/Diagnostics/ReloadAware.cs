using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class ReloadAware<T> : PolicyAware<T>
{
	public ReloadAware(IStopAware<T> previous) : base(previous, ReloadPolicy.Default) {}
}

[UsedImplicitly]
public class ReloadAware<TIn, TOut> : PolicyAware<TIn, TOut>
{
	public ReloadAware(IStopAware<TIn, TOut> previous) : base(previous, ReloadPolicy.Default) {}
}