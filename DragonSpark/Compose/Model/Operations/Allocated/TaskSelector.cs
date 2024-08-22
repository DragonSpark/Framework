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

public class TaskSelector<_, T> : Selector<_, Task<T>>
{
	public TaskSelector(ISelect<_, Task<T>> subject) : base(subject) {}

	public OperationResultComposer<_, T> Structure() => new(Get().Select(SelectOperation<T>.Default));

	public TaskSelector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

	public TaskSelector<_, TTo> Select<TTo>(Func<T, TTo> select)
		=> new(Get().Select(new Selection<T, TTo>(select)));
}


public class TaskSelector<T> : Selector<T, Task>
{
	public TaskSelector(ISelect<T, Task> subject) : base(subject) {}

	public OperationContext<T> Structure() => new(Get().Select(SelectOperation.Default));

	public TaskSelector<T> Append(ISelect<T, Task> command) => Append(command.Await);

	public TaskSelector<T> Append(ICommand<T> command) => Append(command.Execute);

	public TaskSelector<T> Append(Action<T> command) => Append(Start.A.Command(command).Operation().Allocate().Get());

	public TaskSelector<T> Append(DragonSpark.Model.Operations.Allocated.Await<T> command)
		=> new(new DragonSpark.Model.Operations.Allocated.Appending<T>(Get().Await, command));

	public TaskSelector<T> Append(IAllocated command) => Append(command.Await);

	public TaskSelector<T> Append(Await command)
		=> new(new DragonSpark.Model.Operations.Allocated.Termination<T>(Get().Await, command));
}