using DragonSpark.Compose;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Sort<T> : IQuery<T>
	{
		public static Sort<T> Default { get; } = new Sort<T>();

		Sort() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Sorted?.Count > 0
				           ? new(request, DataOperations.PerformSorting(query, request.Sorted), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}
}