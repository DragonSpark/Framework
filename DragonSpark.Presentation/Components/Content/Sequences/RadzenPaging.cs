using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class RadzenPaging<T> : IRadzenPaging<T>
{
	readonly IPages<T> _pages;
	readonly bool       _includeCount;

	public RadzenPaging(IPages<T> pages, bool includeCount = true)
	{
		_pages       = pages;
		_includeCount = includeCount;
	}

	public ulong Count { get; private set; }

	public IEnumerable<T>? Current { get; private set; }

	public async Task Get(LoadDataArgs parameter)
	{
		var input = new PageInput
		{
			Filter  = parameter.Filter,
			OrderBy = parameter.OrderBy,
			Partition = parameter.Skip.HasValue || parameter.Top.HasValue
				            ? new(parameter.Skip, parameter.Top)
				            : null,
			IncludeTotalCount = _includeCount,
		};

		var current      = _pages.Get(input);
		var successfully = current.IsCompletedSuccessfully;
		var evaluate     = successfully ? current.Result : await current.ConfigureAwait(false);
		Current = evaluate;
		Count   = evaluate.Total ?? evaluate.Count.Grade();
	}
}