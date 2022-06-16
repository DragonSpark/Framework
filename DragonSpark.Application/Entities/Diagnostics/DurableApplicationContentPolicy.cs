using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics;

public sealed class DurableApplicationContentPolicy : Deferred<IAsyncPolicy>
{
	public static DurableApplicationContentPolicy Default { get; } = new DurableApplicationContentPolicy();

	DurableApplicationContentPolicy()
		: base(ApplicationContentBuilder.Default.Then().Select(ApplicationContentRetryPolicy.Default)) {}
}