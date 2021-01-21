using DragonSpark.Compose;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Search<T> : IQuery<T>
	{
		public static Search<T> Default { get; } = new Search<T>();

		Search() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Search?.Count > 0
				           ? new(request, DataOperations.PerformSearching(query, request.Search), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}
}