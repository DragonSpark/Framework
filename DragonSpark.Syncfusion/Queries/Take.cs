using DragonSpark.Compose;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Take<T> : IQuery<T>
	{
		public static Take<T> Default { get; } = new Take<T>();

		Take() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Take > 0
				           ? new(request, DataOperations.PerformTake(query, request.Take), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}
}