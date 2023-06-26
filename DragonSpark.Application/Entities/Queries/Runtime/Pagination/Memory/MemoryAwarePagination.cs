using DragonSpark.Application.Compose.Store.Operations;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination.Memory;

sealed class MemoryAwarePagination<T> : IResulting<IPages<T>>
{
	readonly IResulting<IPages<T>>         _pages;
	readonly MemoryStoreProfile<PageInput> _profile;

	public MemoryAwarePagination(IResulting<IPages<T>> pages, MemoryStoreProfile<PageInput> profile)
	{
		_pages   = pages;
		_profile = profile;
	}

	public async ValueTask<IPages<T>> Get()
	{
		var pages = await _pages.Await();
		return pages.IsEmpty() ? pages : new MemoryAwarePages<T>(pages, _profile);
	}
}