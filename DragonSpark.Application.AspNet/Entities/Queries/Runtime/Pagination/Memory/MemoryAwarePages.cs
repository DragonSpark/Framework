using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination.Memory;

public sealed class MemoryAwarePages<T> : StopAware<PageInput, PageResult<T>>, IPages<T>
{
	public MemoryAwarePages(IPages<T> previous, StoreProfile<Stop<PageInput>> profile)
		: base(previous.Then().Store().Using(profile)) {}
}