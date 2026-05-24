using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

readonly struct Loaded<T> : IResult<Task<T?>?>, IDisposable
{
	readonly Task<T?> _previous;
	readonly Switch   _loaded;

	public Loaded(Task<T?> previous) : this(previous, false) {}

	public Loaded(Task<T?> previous, Switch loaded)
	{
		_previous = previous;
		_loaded   = loaded;
	}

	public Task<T?>? Get() => _previous.IsCompleted && _loaded.Up() ? _previous : null;

	public void Dispose()
	{
		_loaded.Down();
		// _ = _previous.ContinueWith(t => _ = t.Exception, TaskContinuationOptions.OnlyOnFaulted);
	}
}