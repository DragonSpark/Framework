using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations.Allocated
{
	public class AllocatedOperationResultSelector<T> : ResultContext<Task<T>>
	{
		public AllocatedOperationResultSelector(IResult<Task<T>> instance) : base(instance) {}

		public OperationResultSelector<T> Structure() => new(Select(x => x.ToOperation()).Get());
	}
}