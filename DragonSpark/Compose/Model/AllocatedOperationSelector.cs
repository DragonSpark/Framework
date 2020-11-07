using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public class AllocatedOperationSelector : ResultContext<Task>
	{
		/*public static implicit operator Operate(OperationSelector instance) => instance.Get().Get;

		public static implicit operator Await(OperationSelector instance) => instance.Get().Await;*/

		public AllocatedOperationSelector(IResult<Task> instance) : base(instance) {}

		public AllocatedOperationSelector Append(Func<Task> next)
			=> new AllocatedOperationSelector(new AllocatedAppended(Get().Get, next));

		public OperationSelector Promote() => new OperationSelector(Select(x => x.ToOperation()).Get());
	}
}