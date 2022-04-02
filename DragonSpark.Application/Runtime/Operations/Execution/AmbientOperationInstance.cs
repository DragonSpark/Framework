using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class AmbientOperationInstance : Logical<object?>
{
	public static AmbientOperationInstance Default { get; } = new();

	AmbientOperationInstance() {}
}