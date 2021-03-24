using DragonSpark.Application.Entities.Diagnostics;
using DragonSpark.Application.Entities.Queries;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Count<T> : IQuery<T>
	{
		readonly EntityQuery<T> _query;
		public static Count<T> Default { get; } = new Count<T>();

		Count() : this(DurableEntityQuery<T>.Default) {}

		public Count(EntityQuery<T> query) => _query  = query;

		public async ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			return count is null && request.RequiresCounts
				       ? new(request, query, await _query.Counting.Get(query))
				       : parameter;
		}
	}
}