using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAwareResult<T> : PolicyAwareResulting<T>
{
	public ConnectionAwareResult(IResulting<T> previous) : base(previous, ConnectionAwarePolicy<T>.Default) {}
}