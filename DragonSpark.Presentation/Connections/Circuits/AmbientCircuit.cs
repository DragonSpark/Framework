using DragonSpark.Runtime.Execution;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class AmbientCircuit : Logical<CircuitRecord>
{
	public static AmbientCircuit Default { get; } = new();

	AmbientCircuit() {}
}