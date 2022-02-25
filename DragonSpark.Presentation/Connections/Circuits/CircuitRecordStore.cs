using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CircuitRecordStore : ReferenceValueTable<Circuit, CircuitRecord>
{
	public static CircuitRecordStore Default { get; } = new();

	CircuitRecordStore() {}
}