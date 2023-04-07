using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class OperationsStore : Logical<IOperations>
{
	public static OperationsStore Default { get; } = new();

	OperationsStore() {}
}