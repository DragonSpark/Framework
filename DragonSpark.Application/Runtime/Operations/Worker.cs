using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public readonly struct Work // TODO: Rename -> Worker
{
	readonly Task _previous;

	public Work(Task previous, Task status)
	{
		_previous = previous;
		Status    = status;
	}

	public Task AsTask() => _previous;

	public Task Status { get; }
}

public sealed record Worker(Task Monitor, ICompleted Complete) : IDisposable
{
	public void Dispose()
	{
		Complete.Dispose();
	}
}