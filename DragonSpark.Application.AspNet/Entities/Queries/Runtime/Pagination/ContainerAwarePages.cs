using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class ContainerAwarePages<T> : IPages<T>
{
	readonly IPageContainer<T> _container;
	readonly IPages<T>         _pages;

	public ContainerAwarePages(IPageContainer<T> container, IPages<T> pages)
	{
		_container = container;
		_pages     = pages;
	}

	public async ValueTask<PageResult<T>> Get(Stop<PageInput> parameter)
	{
		try
		{
			var result = await _pages.On(parameter);
			await _container.On(result);
			return result;
		}
		catch (Exception e)
		{
			await _container.Off(e);
			throw;
		}
	}
}
