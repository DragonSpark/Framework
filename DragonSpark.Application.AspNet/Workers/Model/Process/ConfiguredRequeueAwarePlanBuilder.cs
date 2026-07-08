using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class ConfiguredRequeueAwarePlanBuilder<T> : IPlanBuilder<T> where T : ExternalProcess
{
	readonly IPlanBuilder<T>          _previous;
	readonly IStopAware<MessageInput> _message;
	readonly UpdateStatus             _status;

	protected ConfiguredRequeueAwarePlanBuilder(IPlanBuilder<T> previous, IStopAware<MessageInput> message,
	                                            UpdateStatus status)
	{
		_previous = previous;
		_message  = message;
		_status   = status;
	}

	public IStopAware<T> Get(Array<Step<T>> parameter)
		=> new ConfiguredRequeueAwareStep<T>(_previous.Get(parameter), _message, _status);
}