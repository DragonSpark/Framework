using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class CancelAwarePlanBuilder<T> : IPlanBuilder<T> where T : ExternalProcess
{
	readonly IPlanBuilder<T> _previous;
	readonly UpdateStatus    _status;
	readonly IStopAware<T>   _other;

	protected CancelAwarePlanBuilder(IPlanBuilder<T> previous, UpdateStatus status)
		: this(previous, status, EmptyOperation<T>.Default.AsStop()) {}

	protected CancelAwarePlanBuilder(IPlanBuilder<T> previous, UpdateStatus status, IStopAware<T> other)
	{
		_previous = previous;
		_status   = status;
		_other    = other;
	}

	public IStopAware<T> Get(Array<Step<T>> parameter)
		=> new CancelAwareStep<T>(_previous.Get(parameter), _status, _other);
}