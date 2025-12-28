using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Stop;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class ReloadAware<T> : PolicyAware<T>
{
	public ReloadAware(IStopAware<T> previous) : base(previous, ReloadPolicy.Default.Get()) {}
}

[UsedImplicitly]
public class ReloadAware<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected ReloadAware(ISelecting<TIn, TOut> previous) : base(previous, ReloadPolicy.Default.Get()) {}
}