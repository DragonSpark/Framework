using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ProcessRequest<T> : IDataRequest
{
	readonly Await<Stop<DataManagerRequest>, Page<T>> _current;

	public ProcessRequest(IPages<T> pages) : this(SelectQueryInput.Default.Then().Select(pages).Then()) {}

	public ProcessRequest(Await<Stop<DataManagerRequest>, Page<T>> current) => _current = current;

	public async ValueTask<DataResult> Get(Stop<DataManagerRequest> parameter)
	{
		var evaluate = await _current(parameter);
		return new()  { Result = evaluate, Count = evaluate.Total?.Degrade() ?? -1 };
	}
}