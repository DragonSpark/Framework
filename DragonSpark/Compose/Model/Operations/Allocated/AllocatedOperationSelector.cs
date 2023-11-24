using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations.Allocated;

public class AllocatedOperationSelector : ResultContext<Task>
{
	public AllocatedOperationSelector(IResult<Task> instance) : base(instance) {}

	public AllocatedOperationSelector Append(Func<Task> next) => new(new AllocatedAppended(Get().Get, next));

	public OperationSelector Structure() => new(Select(x => x.ToOperation()).Get());
}