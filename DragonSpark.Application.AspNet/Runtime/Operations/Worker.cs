using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public readonly struct Worker : IAsyncResult, IDisposable
{
	readonly Task _previous;

	public Worker(Task previous, Task status)
	{
		_previous = previous;
		Status    = status;
	}

	public Task AsTask() => _previous;

	public Task Status { get; }

	public object? AsyncState => _previous.AsyncState;

	public WaitHandle AsyncWaitHandle => ((IAsyncResult)_previous).AsyncWaitHandle;

	public bool CompletedSynchronously => ((IAsyncResult)_previous).CompletedSynchronously;

	public bool IsCompleted => _previous.IsCompleted;

	public void Dispose()
	{
		_previous.Dispose();
	}
}

public readonly struct Worker<T> : IAsyncResult, IDisposable
{
	readonly Task _previous;

	public Worker(Task previous, Task<T> status)
	{
		_previous = previous;
		Status    = status;
	}

	public Task AsTask() => _previous;

	public Task<T> Status { get; }

	public object? AsyncState => _previous.AsyncState;

	public WaitHandle AsyncWaitHandle => ((IAsyncResult)_previous).AsyncWaitHandle;

	public bool CompletedSynchronously => ((IAsyncResult)_previous).CompletedSynchronously;

	public bool IsCompleted => _previous.IsCompleted;

	public void Dispose()
	{
		_previous.Dispose();
	}
}