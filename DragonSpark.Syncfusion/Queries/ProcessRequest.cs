using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class ProcessRequest<T> : IDataRequest
{
	readonly Await<DataManagerRequest, Page<T>> _current;

	public ProcessRequest(IPages<T> pages) : this(SelectQueryInput.Default.Then().Select(pages).Then()) {}

	public ProcessRequest(Await<DataManagerRequest, Page<T>> current) => _current = current;

	public async ValueTask<object> Get(DataManagerRequest parameter)
	{
		var evaluate = await _current(parameter);
		var result = evaluate.Total.HasValue
			             ? new DataResult { Result = evaluate, Count = evaluate.Total.Value.Degrade() }
			             : (object)evaluate;
		return result;
	}
}