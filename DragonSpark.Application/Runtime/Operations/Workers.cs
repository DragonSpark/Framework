using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class Workers : ReferenceValueStore<Task, Task>, IAlteration<Task>
{
	public Workers(Action<Task> completed) : base(new ComposeWorkers(completed)) {}
}

/*public sealed class Workers<T> : ReferenceValueTable<IResulting<T?>, Worker>
{
	public Workers(ICompleted<T?> completed) : base(new ComposeWorkers<T>(completed).Get) {}
}*/

public sealed class Workers<T> : ISelect<IResulting<T?>, Worker>
{
	readonly ICompleted<T?> _completed;

	public Workers(ICompleted<T?> completed) => _completed = completed;

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
