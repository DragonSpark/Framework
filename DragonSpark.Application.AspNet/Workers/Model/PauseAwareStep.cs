using DragonSpark.Compose;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class PauseAwareStep<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T> _previous, _other;
	readonly UpdateStatus  _status;

	public PauseAwareStep(IStopAware<T> previous, UpdateStatus status, IStopAware<T> other)
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
		catch (PauseProcessException error)
		{
			var (_, stop) = parameter;
			await _other.Off(parameter);
			await _status.Off(new(new(parameter, ProcessStatus.Paused, error.Reason), stop));
		}
	}
}