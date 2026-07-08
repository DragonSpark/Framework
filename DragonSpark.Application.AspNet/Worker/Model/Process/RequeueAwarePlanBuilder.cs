using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class RequeueAwarePlanBuilder<T> : IPlanBuilder<T> where T : ExternalProcess
{
	readonly IPlanBuilder<T>    _previous;
	readonly IStopAware<string> _send;
	readonly UpdateStatus       _status;

	protected RequeueAwarePlanBuilder(IPlanBuilder<T> previous, IStopAware<string> send, UpdateStatus status)
	{
		_previous = previous;
		_send     = send;
		_status   = status;
	}

	public IStopAware<T> Get(Array<Step<T>> parameter)
		=> new RequeueAwareStep<T>(_previous.Get(parameter), _send, _status);
}