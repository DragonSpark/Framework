using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public class WorkingResult<T> : IWorkingResult<T>
{
	readonly IAllocatedResult<T> _previous;
	readonly Action              _complete;
	readonly IExceptionLogger    _logger;

	public WorkingResult(IResulting<T> previous, Action complete, IExceptionLogger logger)
		: this(previous.Then().Allocate().Out(), complete, logger) {}

	public WorkingResult(IAllocatedResult<T> previous, Action complete, IExceptionLogger logger)
	{
		_previous = previous;
		_complete = complete;
		_logger   = logger;
	}

	public Worker<T> Get()
	{
		var previous = _previous.Get();
		var source   = new TaskCompletionSource<T>();
		var first    = new WorkerOperation<T>(previous, source, _complete);
		var worker   = new TryLogOperation(first, _logger).Get().AsTask();
		return new(worker, source.Task);
	}
}