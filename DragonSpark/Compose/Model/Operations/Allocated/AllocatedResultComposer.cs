using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations.Allocated;

public class AllocatedResultComposer<T> : ResultContext<Task<T>>
{
	public static implicit operator AwaitOf<T>(AllocatedResultComposer<T> instance) => instance.Get().Await;

	public AllocatedResultComposer(IResult<Task<T>> instance) : base(instance) {}

	public OperationResultSelector<T> Structure() => new(Select(x => x.ToOperation()).Get());
}