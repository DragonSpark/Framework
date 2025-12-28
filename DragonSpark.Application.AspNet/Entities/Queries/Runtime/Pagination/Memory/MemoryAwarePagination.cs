using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination.Memory;

public class MemoryAwarePagination<T> : IPagination<T>
{
	readonly StoreProfile<Stop<PageInput>> _profile;

	protected MemoryAwarePagination(StoreProfile<Stop<PageInput>> profile) => _profile = profile;

	public IPages<T> Get(IPages<T> parameter) => new MemoryAwarePages<T>(parameter, _profile);
}