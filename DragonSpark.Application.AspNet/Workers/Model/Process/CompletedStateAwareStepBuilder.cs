using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class CompletedStateAwareStepBuilder<T> : IStepBuilder<T> where T : ExternalProcess
{
	readonly IStepBuilder<T> _previous;

	protected CompletedStateAwareStepBuilder(IStepBuilder<T> previous) => _previous = previous;

	public IStopAware<T> Get(Step<T> parameter)
	{
		var (_, _, identifier) = parameter;
		var previous = _previous.Get(parameter);
		return new CompletedStateAwareStep<T>(previous, identifier);
	}
}