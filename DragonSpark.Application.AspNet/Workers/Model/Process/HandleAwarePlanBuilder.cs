using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class HandleAwarePlanBuilder<T> : IPlanBuilder<T> where T : ExternalProcess
{
	readonly IPlanBuilder<T> _previous;
	readonly IStopAware<T>   _status;

	protected HandleAwarePlanBuilder(IPlanBuilder<T> previous, IEdit edit) : this(previous, new RelayError<T>(edit)) {}

	protected HandleAwarePlanBuilder(IPlanBuilder<T> previous, IStopAware<T> status)
	{
		_previous = previous;
		_status   = status;
	}

	public IStopAware<T> Get(Array<Step<T>> parameter)
	{
		var previous = _previous.Get(parameter);
		return new Handle<T>(previous, _status);
	}
}