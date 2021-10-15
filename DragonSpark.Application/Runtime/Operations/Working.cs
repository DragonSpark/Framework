using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public class Working<TIn, TOut> : IWorking<TIn, TOut>
{
	readonly IAllocating<TIn, TOut> _previous;

	public Working(ISelecting<TIn, TOut> previous) : this(previous.Then().Allocate().Out()) {}

	public Working(IAllocating<TIn, TOut> previous) => _previous = previous;

	public Worker<TOut> Get(TIn parameter)
	{
		var previous = _previous.Get(parameter);
		var source   = new TaskCompletionSource<TOut>();
		var worker   = new WorkerOperation<TOut>(previous, source).Get();
		return new(worker, source.Task);
	}
}

public class Working<T> : IWorking<T>
{
	readonly IAllocated<T> _previous;

	public Working(IOperation<T> previous) : this(previous.Then().Allocate().Out()) {}

	public Working(IAllocated<T> previous) => _previous = previous;

	public Worker Get(T parameter)
	{
		var previous = _previous.Get(parameter);
		var source   = new TaskCompletionSource();
		var worker   = new WorkerOperation(previous, source).Get();
		return new(worker, source.Task);
	}
}