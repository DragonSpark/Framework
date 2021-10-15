using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public class WorkingResult<T> : IWorkingResult<T>
{
	readonly IAllocatedResult<T> _previous;

	public WorkingResult(IResulting<T> previous) : this(previous.Then().Allocate().Out()) {}

	public WorkingResult(IAllocatedResult<T> previous) => _previous = previous;

	public Worker<T> Get()
	{
		var previous = _previous.Get();
		var source   = new TaskCompletionSource<T>();
		var worker   = new WorkerOperation<T>(previous, source).Get();
		return new(worker, source.Task);
	}
}