using DragonSpark.Diagnostics;
using DragonSpark.Model.Results;
using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class ConnectionAwareDisposing : PolicyAwareOperation, IAsyncDisposable
{
	protected ConnectionAwareDisposing(IAsyncDisposable previous) : this(previous, ConnectionAwarePolicy.Default) {}

	protected ConnectionAwareDisposing(IAsyncDisposable previous, IResult<IAsyncPolicy> policy)
		: base(previous.DisposeAsync, policy) {}

	public ValueTask DisposeAsync() => Get();
}