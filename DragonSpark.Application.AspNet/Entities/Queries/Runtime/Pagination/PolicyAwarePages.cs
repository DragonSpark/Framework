using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class PolicyAwarePages<T> : PolicyAwareSelecting<Stop<PageInput>, Page<T>>, IPages<T>
{
	public PolicyAwarePages(IPages<T> previous) : base(previous, DurableConnectionPolicy.Default.Get()) {}
}