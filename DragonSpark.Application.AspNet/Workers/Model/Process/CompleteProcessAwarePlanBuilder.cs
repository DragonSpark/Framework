using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Compose;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class CompleteProcessAwarePlanBuilder<T> : IPlanBuilder<T> where T : ExternalProcess
{
	readonly IPlanBuilder<T> _previous;
	readonly MarkCompleted   _mark;
	readonly IStopAware<T>   _relay;

	// ReSharper disable once TooManyDependencies
	protected CompleteProcessAwarePlanBuilder(IPlanBuilder<T> previous, MarkCompleted mark, IEdit edit,
	                                          string message = "Done!")
		: this(previous, mark, new Relay<T>(new AppendState(ProcessStatus.Completed, message, edit))) {}

	protected CompleteProcessAwarePlanBuilder(IPlanBuilder<T> previous, MarkCompleted mark, IStopAware<T> relay)
	{
		_previous = previous;
		_mark     = mark;
		_relay    = relay;
	}

	public IStopAware<T> Get(Array<Step<T>> parameter)
	{
		var previous = _previous.Get(parameter);
		return previous.Then()
		               .Append(Start.A.Selection<Stop<T>>()
		                            .By.Calling(x => new Stop<ExternalProcess>(x, x))
		                            .Select(_mark)
		                            .Out())
		               .Append(_relay)
		               .Out();
	}
}