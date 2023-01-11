using DragonSpark.Compose;
using Microsoft.JSInterop;
using Polly;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

public sealed class DisposeReference : ConnectionAware
{
	public DisposeReference(IJSObjectReference instance) : this(instance, ConnectionAwarePolicy.Default.Get()) {}

	public DisposeReference(IJSObjectReference instance, IAsyncPolicy policy)
		: base(Start.A.Result<ValueTask>().By.Calling(instance.DisposeAsync).Then().Out(), policy) {}
}