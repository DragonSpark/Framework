using DragonSpark.Application.Entities.Diagnostics;
using DragonSpark.Diagnostics;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class PolicyAwareAny<T> : PolicyAwareSelecting<AnyInput<T>, bool>, IAny<T>
{
	public PolicyAwareAny(IAny<T> previous) : base(previous, DurableConnectionPolicy.Default.Get()) {}
}