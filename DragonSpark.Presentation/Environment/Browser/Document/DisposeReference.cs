using DragonSpark.Compose;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

public sealed class DisposeReference : ConnectionAware
{
	public DisposeReference(IJSObjectReference instance)
		: base(Start.A.Result<ValueTask>().By.Calling(instance.DisposeAsync).Then().Out()) {}
}