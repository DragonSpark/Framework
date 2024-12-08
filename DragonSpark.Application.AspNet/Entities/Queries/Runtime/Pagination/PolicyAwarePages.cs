using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class PolicyAwarePages<T> : PolicyAwareSelecting<PageInput, Page<T>>, IPages<T>
{
	public PolicyAwarePages(IPages<T> previous) : base(previous, DurableConnectionPolicy.Default.Get()) {}
}