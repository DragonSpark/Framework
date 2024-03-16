using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public sealed class ContainerAwarePages<T> : IPages<T>
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
		var result = await _pages.Get(parameter);
		_container.Execute(result);
		return result;
	}
}