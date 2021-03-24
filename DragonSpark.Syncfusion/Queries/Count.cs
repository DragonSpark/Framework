using DragonSpark.Application.Entities.Queries;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Count<T> : IQuery<T>
	{
		readonly ApplicationQuery<T> _query;
		public static Count<T> Default { get; } = new Count<T>();

		Count() : this(ApplicationQuery<T>.Default) {}

		public Count(ApplicationQuery<T> query) => _query  = query;

		public async ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			return count is null && request.RequiresCounts
				       ? new(request, query, (uint)await _query.Count.Get(query))
				       : parameter;
		}
	}
}