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

sealed class ComposePage<TIn, T> : IStopAware<DataManagerRequest, DataResult> where TIn : PageInput
{
	readonly Func<Stop<TIn>, Task<Page<T>>>         _page;
	readonly ISelect<DataManagerRequest, PageInput> _select;

	public ComposePage(Func<Stop<TIn>, Task<Page<T>>> page, ISelect<DataManagerRequest, PageInput> select)
	{
		_page   = page;
		_select = select;
	}

	public async ValueTask<DataResult> Get(Stop<DataManagerRequest> parameter)
	{
		var (subject, stop) = parameter;
		var input = _select.Get(subject);
		var page  = await _page(new((TIn)input, stop)).Off();
		return new() { Result = page, Count = page.Total?.Degrade() ?? page.Count };
	}
}