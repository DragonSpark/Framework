using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class Workers : ReferenceValueStore<Task, Task>, IAlteration<Task>
{
	public Workers(Action<Task> completed) : base(new ComposeWorkers(completed)) {}
}

public sealed class Workers<T> : ReferenceValueStore<IResulting<T?>, Worker>
{
	public Workers(ICompleted<T?> completed) : base(new ComposeWorkers<T>(completed)) {}
}