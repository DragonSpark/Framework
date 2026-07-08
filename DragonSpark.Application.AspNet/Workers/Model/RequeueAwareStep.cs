using DragonSpark.Compose;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class RequeueAwareStep<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T>      _previous;
	readonly IStopAware<string> _send;
	readonly UpdateStatus       _status;

	public RequeueAwareStep(IStopAware<T> previous, IStopAware<string> send, UpdateStatus status)
	{
		_previous = previous;
		_send     = send;
		_status   = status;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch (RequeueProcessException error)
		{
			var (subject, stop) = parameter;
			await _send.Off(new(subject.Id.ToString(), stop));
			await _status.Off(new(new(subject, ProcessStatus.Queued, error.Reason), stop));
		}
	}
}