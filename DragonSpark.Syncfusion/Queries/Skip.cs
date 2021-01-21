using DragonSpark.Compose;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Skip<T> : IQuery<T>
	{
		public static Skip<T> Default { get; } = new Skip<T>();

		Skip() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Skip > 0
				           ? new(request, DataOperations.PerformSkip(query, request.Skip), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}
}