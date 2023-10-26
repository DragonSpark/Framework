using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Diagnostics;

public class ReloadAware<T> : PolicyAwareOperation<T>
{
	public ReloadAware(IOperation<T> previous) : base(previous, ConcurrencyAwarePolicy.Default.Get()) {}
}