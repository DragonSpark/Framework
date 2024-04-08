using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination.Memory;

public sealed class MemoryAwarePages<T> : Selecting<PageInput, Page<T>>, IPages<T>
{
	public MemoryAwarePages(IPages<T> previous, StoreProfile<PageInput> profile)
		: base(previous.Then().Store().Using(profile)) {}
}