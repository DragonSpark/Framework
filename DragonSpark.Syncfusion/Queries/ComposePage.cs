using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ComposePage<T> : IStopAware<DataManagerRequest, DataResult>
{
	readonly Func<Stop<PageInput>, Task<Page<T>>>   _page;
	readonly ISelect<DataManagerRequest, PageInput> _select;

	public ComposePage(Func<Stop<PageInput>, Task<Page<T>>> page) : this(page, SelectQueryInput.Default) {}

	public ComposePage(Func<Stop<PageInput>, Task<Page<T>>> page, ISelect<DataManagerRequest, PageInput> select)
	{
		_page   = page;
		_select = select;
	}

	public async ValueTask<DataResult> Get(Stop<DataManagerRequest> parameter)
	{
		var (subject, stop) = parameter;
		var page = await _page(new(_select.Get(subject), stop)).Off();
		return new ()
		{
			Result = page, Count = page.Total?.Degrade() ?? page.Count
		};
	}
}