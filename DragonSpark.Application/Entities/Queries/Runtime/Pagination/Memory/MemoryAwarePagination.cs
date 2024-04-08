using DragonSpark.Application.Compose.Store.Operations.Memory;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination.Memory;

public class MemoryAwarePagination<T> : IPagination<T>
{
	readonly StoreProfile<PageInput> _profile;

	protected MemoryAwarePagination(StoreProfile<PageInput> profile) => _profile = profile;

	public IPages<T> Get(IPages<T> parameter) => new MemoryAwarePages<T>(parameter, _profile);
}