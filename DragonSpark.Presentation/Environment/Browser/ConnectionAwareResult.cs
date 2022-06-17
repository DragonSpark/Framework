using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAwareResult<T> : PolicyAwareResulting<T>
{
	protected ConnectionAwareResult(IResulting<T> previous) : base(previous, ConnectionAwarePolicy<T>.Default) {}
}