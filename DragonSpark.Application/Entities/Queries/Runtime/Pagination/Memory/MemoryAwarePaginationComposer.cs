using DragonSpark.Application.Compose.Store.Operations;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination.Memory;

public class MemoryAwarePaginationComposer<T> : IPaginationComposer<T>
{
	readonly MemoryStoreProfile<PageInput> _profile;

	protected MemoryAwarePaginationComposer(MemoryStoreProfile<PageInput> profile) => _profile = profile;

	public IResulting<IPages<T>> Get(IResulting<IPages<T>> parameter)
		=> new MemoryAwarePagination<T>(parameter, _profile);
}