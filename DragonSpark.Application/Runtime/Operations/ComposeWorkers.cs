using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using System;
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
		var monitor  = new Monitor<T>(task, complete).Get();
		return new(monitor, complete);
	}
}

sealed class ComposeWorkers : IAlteration<Task>
{
	readonly Action<Task> _completed;

	public ComposeWorkers(Action<Task> completed) => _completed = completed;

	public Task Get(Task parameter) => new Monitor(parameter, _completed).Get();
}