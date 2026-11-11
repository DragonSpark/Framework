using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Contracts.Queries;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class Filtered : IFiltered
{
	readonly IFiltered _previous;

	public Filtered(IFiltered previous) => _previous = previous;

	public bool Get(PageInput parameter)
	{
		var previous = _previous.Get(parameter);

		return !previous && parameter is SyncfusionPageInput { Request: var request }
			       ? request.Sorted?.Any() == true || request.Where?.Any() == true || request.Search?.Any() == true
			       : previous;
	}
}