using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Contracts.Queries;
using DragonSpark.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class PolicyAwarePages<T> : PolicyAware<PageInput, PageResult<T>>, IPages<T>
{
	public PolicyAwarePages(IPages<T> previous) : base(previous, DurableConnectionPolicy.Default) {}
}