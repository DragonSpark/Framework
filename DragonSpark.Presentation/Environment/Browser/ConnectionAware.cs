using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAware : PolicyAwareOperation
{
	protected ConnectionAware(IOperation previous) : base(previous, ConnectionAwarePolicy.Default) {}
}

public class ConnectionAware<T> : PolicyAwareOperation<T>
{
	public ConnectionAware(IOperation<T> previous) : base(previous, ConnectionAwarePolicy.Default) {}
}