using DragonSpark.Model.Results;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class DataRequestResult(DataManagerRequest request, string? key = null) : Variable<object>
{
	public DataManagerRequest Request { get; } = request;

	public string? Key { get; } = key;
}