using DragonSpark.Model.Results;
using DragonSpark.Presentation.Connections.Circuits;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Presentation.Environment;

sealed class CurrentCircuitStore : Logical<IMutable<CircuitRecord?>>
{
	public static CurrentCircuitStore Default { get; } = new();

	CurrentCircuitStore() {}
}