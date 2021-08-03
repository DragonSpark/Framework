using DragonSpark.Compose.Model.Operations.Allocated;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations
{
	public class OperationSelector : ResultContext<ValueTask>
	{
		public static implicit operator Operate(OperationSelector instance) => instance.Get().Get;

		public static implicit operator Await(OperationSelector instance) => instance.Get().Await;

		public OperationSelector(IResult<ValueTask> instance) : base(instance) {}

		public OperationSelector Append(Await next) => new OperationSelector(new Appending(Get().Await, next));

		public OperationSelector Append(Operate next) => new OperationSelector(new AppendedOperate(Get().Get, next));

		public AllocatedOperationSelector Allocate() => new AllocatedOperationSelector(Select(x => x.AsTask()).Get());
	}
}