using DragonSpark.Model.Selection.Alterations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

sealed class ComposeWorkers : IAlteration<Task>
{
	readonly Action<Task> _completed;

	public ComposeWorkers(Action<Task> completed) => _completed = completed;

	public Task Get(Task parameter) => new Monitor(parameter, _completed).Get();
}