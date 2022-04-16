using DragonSpark.Model.Results;
using DragonSpark.Presentation.Environment;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CurrentCircuit : AssumedVariableStore<CircuitRecord?>
{
	public static CurrentCircuit Default { get; } = new();

	CurrentCircuit() : base(CurrentCircuitStore.Default) {}
}