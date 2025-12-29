using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ProcessRequest<T> : IDataRequest
{
	readonly IPages<T>                              _pages;
	readonly ISelect<DataManagerRequest, PageInput> _select;

	public ProcessRequest(IPages<T> pages) : this(pages, ComposePageInput.Default) {}

	public ProcessRequest(IPages<T> pages, ISelect<DataManagerRequest, PageInput> select)
	{
		_pages  = pages;
		_select = select;
	}

	public async ValueTask<DataResult> Get(Stop<DataManagerRequest> parameter)
	{
		var (subject, stop) = parameter;
		var input = _select.Get(subject);
		var (page, total) = await _pages.Off(new(input, stop));
		return new() { Result = page, Count = total?.Degrade() ?? -1 };
	}
}