using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations.Allocated;

public class AllocatedOperationSelector : ResultContext<Task>
{
	public AllocatedOperationSelector(IResult<Task> instance) : base(instance) {}

	public AllocatedOperationSelector Append(Func<Task> next)
		=> new AllocatedOperationSelector(new AllocatedAppended(Get().Get, next));

	public OperationSelector Structure() => new OperationSelector(Select(x => x.ToOperation()).Get());
}