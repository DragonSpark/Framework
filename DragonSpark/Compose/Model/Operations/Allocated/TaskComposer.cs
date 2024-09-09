using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;
using Await = DragonSpark.Model.Operations.Allocated.Await;

namespace DragonSpark.Compose.Model.Operations.Allocated;

public class TaskComposer<_, T> : Composer<_, Task<T>>
{
	public TaskComposer(ISelect<_, Task<T>> subject) : base(subject) {}

	public OperationResultComposer<_, T> Structure() => new(Get().Select(SelectOperation<T>.Default));

	public TaskComposer<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

	public TaskComposer<_, TTo> Select<TTo>(Func<T, TTo> select)
		=> new(Get().Select(new Selection<T, TTo>(select)));
}


public class TaskComposer<T> : Composer<T, Task>
{
	public TaskComposer(ISelect<T, Task> subject) : base(subject) {}

	public OperationComposer<T> Structure() => new(Get().Select(SelectOperation.Default));

	public TaskComposer<T> Append(ISelect<T, Task> command) => Append(command.Await);

	public TaskComposer<T> Append(ICommand<T> command) => Append(command.Execute);

	public TaskComposer<T> Append(Action<T> command) => Append(Start.A.Command(command).Operation().Allocate().Get());

	public TaskComposer<T> Append(DragonSpark.Model.Operations.Allocated.Await<T> command)
		=> new(new DragonSpark.Model.Operations.Allocated.Appending<T>(Get().Await, command));

	public TaskComposer<T> Append(IAllocated command) => Append(command.Await);

	public TaskComposer<T> Append(Await command)
		=> new(new DragonSpark.Model.Operations.Allocated.Termination<T>(Get().Await, command));
}