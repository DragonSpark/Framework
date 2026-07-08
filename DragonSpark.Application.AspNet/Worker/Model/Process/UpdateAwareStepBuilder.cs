using DragonSpark.Application.AspNet.Worker.Model.Process.States;
using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class UpdateAwareStepBuilder<T> : IStepBuilder<T> where T : ExternalProcess
{
	readonly IStepBuilder<T> _previous;
	readonly IEdit           _edit;

	protected UpdateAwareStepBuilder(IStepBuilder<T> previous, IEdit edit)
	{
		_previous = previous;
		_edit     = edit;
	}

	public IStopAware<T> Get(Step<T> parameter)
	{
		var (_, message, _) = parameter;
		var update   = new Update(ProcessStatus.Processing, message);
		var previous = _previous.Get(parameter);
		return new UpdateAwareStep<T>(update, _edit, previous);
	}
}