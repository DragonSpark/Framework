using DragonSpark.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class Operations : IOperations
{
	readonly IList<Task> _tasks;

	public Operations() : this(new List<Task>()) {}

	public Operations(IList<Task> tasks) => _tasks = tasks;

	public void Execute(Task parameter)
	{
		_tasks.Add(parameter);
	}

	public Task Get() => _tasks.Count > 1 ? Task.WhenAll(_tasks) : _tasks[0];

	public void Execute(None parameter)
	{
		_tasks.Clear();
	}

	public bool Get(None parameter) => _tasks.Count > 0;
}