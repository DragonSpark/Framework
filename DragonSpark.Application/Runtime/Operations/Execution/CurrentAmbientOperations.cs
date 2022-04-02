using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class CurrentAmbientOperations : SelectedResult<object, IOperations>
{
	public static CurrentAmbientOperations Default { get; } = new();

	CurrentAmbientOperations() : base(A.Result(AmbientOperationInstance.Default).Then().Select(x => x.Verify()).Get(),
	                                  InstanceOperations.Default) {}
}