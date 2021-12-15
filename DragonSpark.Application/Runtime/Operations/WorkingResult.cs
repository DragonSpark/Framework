using DragonSpark.Application.Diagnostics;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public class WorkingResult<T> : IWorkingResult<T>
{
	readonly IResulting<T>    _previous;
	readonly Action           _complete;
	readonly IExceptionLogger _logger;

	public WorkingResult(IResulting<T> previous, Action complete, IExceptionLogger logger)
	{
		_previous = previous;
		_complete = complete;
		_logger   = logger;
	}

	public Worker<T> Get()
	{
		var previous = _previous.Get();
		if (previous.IsCompletedSuccessfully)
		{
			return new(Task.CompletedTask, previous.AsTask());
		}
		var source = new TaskCompletionSource<T>();
		var first  = new WorkerOperation<T>(previous.AsTask(), source, _complete);
		var worker = new TryLogOperation(first, _logger).Get().AsTask();
		return new(worker, source.Task);
	}
}