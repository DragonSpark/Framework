using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using System;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Components;

sealed class PagedResult<T> : IAllocating<Stop<PageRequest>, Page<T>>
{
	readonly Func<Stop<PageRequest>, Task<PageResult<T>>> _previous;

	public PagedResult(Func<Stop<PageRequest>, Task<PageResult<T>>> previous) => _previous = previous;

	public async Task<Page<T>> Get(Stop<PageRequest> parameter)
	{
		var (previous, count) = await _previous(parameter).Off();
		return new(previous, count);
	}
}