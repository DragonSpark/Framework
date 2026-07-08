using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class Handle<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T> _previous, _status;

	public Handle(IStopAware<T> previous, IStopAware<T> status)
	{
		_previous = previous;
		_status   = status;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		try
		{
			await _previous.Off(parameter);
		}
		catch
		{
			await _status.Off(parameter);
			throw;
		}
	}
}