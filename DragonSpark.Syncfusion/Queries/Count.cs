using DragonSpark.Presentation.Components.Content;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class Count<T> : IQuery<T>
	{
		readonly IAsyncPolicy _policy;
		public static Count<T> Default { get; } = new Count<T>();

		Count() : this(DurableApplicationContentPolicy.Default.Get()) {}

		public Count(IAsyncPolicy policy) => _policy = policy;

		public async ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			return count is null && request.RequiresCounts
				       ? new(request, query, (uint)await _policy.ExecuteAsync(() => query.CountAsync()))
				       : parameter;
		}
	}
}