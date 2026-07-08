using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Compose;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

sealed class CancelAwareStep<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T> _previous, _other;
	readonly UpdateStatus  _status;

	public CancelAwareStep(IStopAware<T> previous, UpdateStatus status, IStopAware<T> other)
	{
		_previous = previous;
		_status   = status;
		_other    = other;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch (CancelProcessException error)
		{
			var (subject, stop) = parameter;
			await _other.Off(parameter);
			await _status.Off(new(new(subject, ProcessStatus.Canceled, error.Reason), stop));
		}
	}
}