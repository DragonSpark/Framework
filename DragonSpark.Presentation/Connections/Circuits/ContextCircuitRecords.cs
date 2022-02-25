using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class ContextCircuitRecords : ReferenceValueTable<object, CircuitRecord>
{
	public static ContextCircuitRecords Default { get; } = new();

	ContextCircuitRecords() {}
}