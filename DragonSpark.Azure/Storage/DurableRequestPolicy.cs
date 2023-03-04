using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Azure.Storage;

public sealed class DurableRequestPolicy<T> : Deferred<IAsyncPolicy<T>>
{
	public static DurableRequestPolicy<T> Default { get; } = new();

	DurableRequestPolicy() : base(RequestBuilder<T>.Default.Then().Select(DefaultRetryPolicy<T>.Default)) {}
}