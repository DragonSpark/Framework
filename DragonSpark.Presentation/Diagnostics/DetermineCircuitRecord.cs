using DragonSpark.Model.Results;
using DragonSpark.Presentation.Connections.Circuits;

namespace DragonSpark.Presentation.Diagnostics;

sealed class DetermineCircuitRecord : Maybe<CircuitRecord>
{
	public static DetermineCircuitRecord Default { get; } = new();

	DetermineCircuitRecord() : base(AmbientCircuit.Default, CurrentCircuitRecord.Default) {}
}