using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Count<T> : IQuery<T>
	{
		readonly ICount<T> _query;

		public Count(ICount<T> query) => _query = query;

		public async ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			return count is null && request.RequiresCounts
				       ? new(request, query, await _query.Get(query))
				       : parameter;
		}
	}
}