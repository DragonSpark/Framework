using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;

namespace DragonSpark.Compose.Model.Operations.Allocated;

public class AllocatedOperationComposer : ResultComposer<Task>
{
	public AllocatedOperationComposer(IResult<Task> instance) : base(instance) {}

	public AllocatedOperationComposer Append(Func<Task> next) => new(new AllocatedAppended(Get().Get, next));

	public OperationComposer Structure() => new(Select(static x => x.ToOperation()).Get());
}