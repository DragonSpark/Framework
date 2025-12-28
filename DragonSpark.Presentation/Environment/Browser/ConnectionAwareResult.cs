using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Results.Stop;
using JetBrains.Annotations;

namespace DragonSpark.Presentation.Environment.Browser;

[UsedImplicitly]
public class ConnectionAwareResult<T> : PolicyAwareResulting<T>
{
	public ConnectionAwareResult(IStopAware<T> previous) : base(previous, ConnectionAwarePolicy<T>.Default) {}
}