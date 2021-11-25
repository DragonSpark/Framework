using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public class WorkingResult<T> : IWorkingResult<T>
{
	readonly IAllocatedResult<T> _previous;
	readonly IExceptionLogger    _logger;

	public WorkingResult(IResulting<T> previous, IExceptionLogger logger)
		: this(previous.Then().Allocate().Out(), logger) {}

	public WorkingResult(IAllocatedResult<T> previous, IExceptionLogger logger)
	{
		_previous    = previous;
		_logger = logger;
	}

	public Worker<T> Get()
	{
		var previous = _previous.Get();
		var source   = new TaskCompletionSource<T>();
		var worker   = new WorkerOperation<T>(previous, source, _logger).Get();
		return new(worker, source.Task);
	}
}