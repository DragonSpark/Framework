using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

sealed class ComposeWorkers<T> : ISelect<IResulting<T?>, Worker>
{
	readonly ICompleted<T?> _completed;

	public ComposeWorkers(ICompleted<T?> completed) => _completed = completed;

	public Worker Get(IResulting<T?> parameter)
	{
		var previous = parameter.Get();
		if (previous.IsCompletedSuccessfully)
		{
			return new(Task.CompletedTask, new Completed<T?>(parameter, _completed, previous.Result));
		}

		var task     = previous.AsTask();
		var complete = new Complete<T?>(parameter, _completed, task);
		var monitor  = new WorkerMonitor<T>(task, complete).Get();
		return new(monitor, complete);
	}
}