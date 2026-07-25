using DragonSpark.Compose;
using Microsoft.JSInterop;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser.Document;

public sealed class DisposeReference : ConnectionAwareOperation
{
	public DisposeReference(IJSObjectReference instance) : this(instance, ConnectionAwarePolicy.Default.Get()) {}

	public DisposeReference(IJSObjectReference instance, IAsyncPolicy policy)
		: base(Start.A.Result<ValueTask>().By.Calling(instance.DisposeAsync).Then().Out(), policy) {}
}