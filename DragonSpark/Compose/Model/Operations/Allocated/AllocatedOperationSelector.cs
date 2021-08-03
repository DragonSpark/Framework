using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations.Allocated
{
	public class AllocatedOperationSelector : ResultContext<Task>
	{
		public AllocatedOperationSelector(IResult<Task> instance) : base(instance) {}

		public AllocatedOperationSelector Append(Func<Task> next)
			=> new AllocatedOperationSelector(new AllocatedAppended(Get().Get, next));

		public OperationSelector Structure() => new OperationSelector(Select(x => x.ToOperation()).Get());
	}

	public class AllocatedOperationResultSelector<T> : ResultContext<Task<T>>
	{
		public AllocatedOperationResultSelector(IResult<Task<T>> instance) : base(instance) {}

		public OperationResultSelector<T> Structure() => new(Select(x => x.ToOperation()).Get());
	}
}