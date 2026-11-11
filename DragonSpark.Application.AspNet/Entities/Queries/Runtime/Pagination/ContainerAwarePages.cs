using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class ContainerAwarePages<T> : IPages<T>
{
	readonly IPageContainer<T> _container;
	readonly IPages<T>         _pages;
	readonly IFiltered         _filtered;

	public ContainerAwarePages(IPageContainer<T> container, IPages<T> pages, IFiltered filtered)
	{
		_container = container;
		_pages     = pages;
		_filtered  = filtered;
	}

	public async ValueTask<PageResult<T>> Get(Stop<PageInput> parameter)
	{
		try
		{
			var (subject, _) = parameter;
			var result = await _pages.On(parameter);
			await _container.On(new PageStatus<T>(result, _filtered.Get(subject)));
			return result;
		}
		catch (Exception e)
		{
			await _container.Off(e);
			throw;
		}
	}
}