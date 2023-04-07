using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class NewOperations : IResult<IOperations>
{
	public static NewOperations Default { get; } = new();

	NewOperations() {}

	public IOperations Get()
	{
		var queue  = new DeferredOperationsQueue();
		var result = new Operations(new ProcessOperations(queue), queue);
		return result;
	}
}