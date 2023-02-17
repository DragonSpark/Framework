using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAware : PolicyAwareOperation
{
	public ConnectionAware(IOperation previous) : this(previous, ConnectionAwarePolicy.Default.Get()) {}

	protected ConnectionAware(IOperation previous, IAsyncPolicy policy) : base(previous, policy) {}
}

public class ConnectionAware<T> : PolicyAwareOperation<T>
{
	public ConnectionAware(IOperation<T> previous) : base(previous, ConnectionAwarePolicy.Default) {}
}