using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Stop;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAware : PolicyAware
{
	public ConnectionAware(IStopAware previous) : this(previous, ConnectionAwarePolicy.Default.Get()) {}

	protected ConnectionAware(IStopAware previous, IAsyncPolicy policy) : base(previous, policy) {}
}

public class ConnectionAware<T> : PolicyAware<T>
{
	public ConnectionAware(IStopAware<T> previous) : base(previous, ConnectionAwarePolicy.Default) {}
}