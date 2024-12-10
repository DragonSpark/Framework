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

	public async ValueTask<Page<T>> Get(PageInput parameter)
	{
		try
		{
			var result = await _pages.Get(parameter).ConfigureAwait(true);
			_container.Execute(result);
			return result;
		}
		catch (Exception e)
		{
			_container.Execute(e);
			throw;
		}
	}
}
