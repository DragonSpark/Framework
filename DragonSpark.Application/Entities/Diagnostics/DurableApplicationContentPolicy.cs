using DragonSpark.Compose;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Polly;

namespace DragonSpark.Application.Entities.Diagnostics;

public sealed class DurableApplicationContentPolicy : Deferred<IAsyncPolicy>
{
	[UsedImplicitly]
	public static DurableApplicationContentPolicy Default { get; } = new();

	DurableApplicationContentPolicy()
		: base(ApplicationContentBuilder.Default.Then().Select(ApplicationContentRetryPolicyBuilder.Default)) {}
}