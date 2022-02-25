using DragonSpark.Model.Selection.Stores;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CircuitRecordIdentification : ConcurrentTable<string, CircuitRecord>
{
	public static CircuitRecordIdentification Default { get; } = new();

	CircuitRecordIdentification() : base(new ConcurrentDictionary<string, CircuitRecord>()) {}
}