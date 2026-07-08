using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class ConfiguredRequeueAwareStep<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T>            _previous;
	readonly IStopAware<MessageInput> _message;
	readonly UpdateStatus             _status;

	public ConfiguredRequeueAwareStep(IStopAware<T> previous, IStopAware<MessageInput> message, UpdateStatus status)
	{
		_previous = previous;
		_message  = message;
		_status   = status;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch (ConfiguredRequeueProcessException error)
		{
			var (subject, stop) = parameter;
			await _message.Off(new(new(subject.Id.ToString(), error.Visibility, error.Life), stop));
			await _status.Off(new(new(subject, ProcessStatus.Queued, error.Reason), stop));
		}
	}
}