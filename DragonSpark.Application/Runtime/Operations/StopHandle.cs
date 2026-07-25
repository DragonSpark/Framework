using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class StopHandle : IStopHandle
{
	readonly IMutable<CancellationTokenSource?> _store;
	readonly IResult<CancellationTokenSource>   _source;

	public StopHandle() : this(new Variable<CancellationTokenSource>(new CancellationTokenSource())) {}

	public StopHandle(IMutable<CancellationTokenSource?> store) : this(store, new TokenSource(store)) {}

	public StopHandle(IMutable<CancellationTokenSource?> store, IResult<CancellationTokenSource> source)
	{
		_store  = store;
		_source = source;
	}

	public async ValueTask Get()
	{
		if (_store.TryPop(out var source))
		{
			await source.CancelAsync().Off();
		}
	}

	public CancellationToken Token => _source.Get().Token;
}