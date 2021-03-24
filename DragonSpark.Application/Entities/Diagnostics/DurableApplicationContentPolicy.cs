using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics
{
	public sealed class DurableApplicationContentPolicy : DeferredSingleton<IAsyncPolicy>
	{
		public static DurableApplicationContentPolicy Default { get; } = new DurableApplicationContentPolicy();

		DurableApplicationContentPolicy()
			: base(ApplicationContentPolicy.Default.Then().Select(ApplicationContentRetryPolicy.Default)) {}
	}
}