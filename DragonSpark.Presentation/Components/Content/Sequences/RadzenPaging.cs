using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using Radzen;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class RadzenPaging<T> : IRadzenPaging<T>
{
	readonly IPages<T> _pages;
	readonly bool      _includeCount;

	public RadzenPaging(IPages<T> pages, bool includeCount = true)
	{
		_pages        = pages;
		_includeCount = includeCount;
	}

	public ulong Count { get; private set; }

	public IEnumerable<T>? Current { get; private set; }

	public async Task Get(Stop<LoadDataArgs> parameter)
	{
		var (subject, stop) = parameter;
		var input = new PageInput(_includeCount, subject.OrderBy, subject.Filter,
		                          subject.Skip.HasValue || subject.Top.HasValue
			                          ? new(subject.Skip, subject.Top)
			                          : null);

		var current      = _pages.Get(new(input, stop));
		var successfully = current.IsCompletedSuccessfully;
		var (page, total) = successfully ? current.Result : await current.Off();
		Current           = page;
		Count             = total ?? page.Length.Grade();
	}
}