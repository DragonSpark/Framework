using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Invocation.Operations
{
	sealed class TaskSelector : Select<Action, Task>
	{
		public static TaskSelector Default { get; } = new TaskSelector();

		TaskSelector() : base(Task.Run) {}
	}

	sealed class TaskSelector<T> : Select<Func<T>, Task<T>>
	{
		public static TaskSelector<T> Default { get; } = new TaskSelector<T>();

		TaskSelector() : base(Task.Run) {}
	}
}