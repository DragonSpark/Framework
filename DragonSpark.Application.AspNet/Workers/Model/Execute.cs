using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class Execute<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T> _previous;
	readonly Guid          _identifier;

	public Execute(IStopAware<T> previous, Guid identifier)
	{
		_previous   = previous;
		_identifier = identifier;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, _) = parameter;
		await _previous.Off(parameter);
		subject.CompletedSteps.Add(new() { Identifier = _identifier });
	}
}