using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class DataRequest : IDataRequest
{
	readonly Func<Stop<DataManagerRequest>, Task<DataResult>> _select;

	public DataRequest(Func<Stop<DataManagerRequest>, Task<DataResult>> select) => _select = select;

	public async ValueTask<DataResult> Get(Stop<DataManagerRequest> parameter) => await _select(parameter).Off();
}