using DragonSpark.Model.Results;
using Syncfusion.Blazor;

namespace DragonSpark.Syncfusion.Queries;

public sealed class DataRequestResult : Variable<object>
{
	public DataRequestResult(DataManagerRequest request, string? key = null)
	{
		Request = request;
		Key     = key;
	}

	public DataManagerRequest Request { get; }

	public string? Key { get; }
}