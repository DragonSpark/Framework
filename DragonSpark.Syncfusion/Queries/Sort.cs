using DragonSpark.Compose;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class Sort<T> : IQuery<T>
{
	public static Sort<T> Default { get; } = new();

	Sort() {}

	public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
	{
		var (request, query) = parameter;
		var data = request.Sorted?.Count > 0
			           ? new(request, DataOperations.PerformSorting(query, request.Sorted))
			           : parameter;
		var result = data.ToOperation();
		return result;
	}
}