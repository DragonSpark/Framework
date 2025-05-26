using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

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
			                          : null, stop);

		var current      = _pages.Get(input);
		var successfully = current.IsCompletedSuccessfully;
		var evaluate     = successfully ? current.Result : await current.Off();
		Current = evaluate;
		Count   = evaluate.Total ?? evaluate.Count.Grade();
	}
}