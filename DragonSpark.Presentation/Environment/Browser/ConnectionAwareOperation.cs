using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using Polly;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAwareOperation<T> : PolicyAwareOperation<T>
{
	public ConnectionAwareOperation(IOperation<T> previous) : base(previous, ConnectionAwarePolicy.Default) {}
}

public class ConnectionAwareOperation : PolicyAwareOperation
{
	public ConnectionAwareOperation(IOperation previous) : this(previous, ConnectionAwarePolicy.Default.Get()) {}

	protected ConnectionAwareOperation(IOperation previous, IAsyncPolicy policy) : base(previous, policy) {}
}