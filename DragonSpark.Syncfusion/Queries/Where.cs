using DragonSpark.Compose;
using Syncfusion.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Where<T> : IQuery<T>
	{
		public static Where<T> Default { get; } = new Where<T>();

		Where() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, _) = parameter;
			var data = request.Where?.Count > 0
				           ? new(request,
				                 DataOperations.PerformFiltering(query, request.Where, request.Where[0].Operator))
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}
}