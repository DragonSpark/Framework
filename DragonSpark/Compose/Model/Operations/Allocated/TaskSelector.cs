using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations.Allocated;

public class TaskSelector<_, T> : Selector<_, Task<T>>
{
	public TaskSelector(ISelect<_, Task<T>> subject) : base(subject) {}

	public OperationResultSelector<_, T> Structure() => new(Get().Select(SelectOperation<T>.Default));

	public TaskSelector<_, TTo> Select<TTo>(ISelect<T, TTo> select) => Select(select.Get);

	public TaskSelector<_, TTo> Select<TTo>(Func<T, TTo> select)
		=> new(Get().Select(new Selection<T, TTo>(select)));
}


public class TaskSelector<T> : Selector<T, Task>
{
	public TaskSelector(ISelect<T, Task> subject) : base(subject) {}

	public OperationContext<T> Structure() => new(Get().Select(SelectOperation.Default));
}