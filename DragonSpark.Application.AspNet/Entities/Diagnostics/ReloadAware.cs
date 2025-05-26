using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public sealed class ReloadAware<T> : PolicyAwareOperation<T>
{
	public ReloadAware(IOperation<T> previous) : base(previous, ReloadPolicy.Default.Get()) {}
}

[UsedImplicitly]
public class ReloadAware<TIn, TOut> : PolicyAwareSelecting<TIn, TOut>
{
	protected ReloadAware(ISelecting<TIn, TOut> previous) : base(previous, ReloadPolicy.Default.Get()) {}
}