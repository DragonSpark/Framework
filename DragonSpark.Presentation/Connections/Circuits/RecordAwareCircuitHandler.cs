using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class RecordAwareCircuitHandler : CircuitHandler
{
	readonly RegisterCircuit                 _register;
	readonly ITable<string, CircuitRecord>   _identification;
	readonly ISelect<Circuit, CircuitRecord> _records;

	public RecordAwareCircuitHandler(RegisterCircuit register)
		: this(register, CircuitRecordIdentification.Default, CircuitRecords.Default) {}

	public RecordAwareCircuitHandler(RegisterCircuit register, ITable<string, CircuitRecord> identification,
	                                 ISelect<Circuit, CircuitRecord> records)
	{
		_register       = register;
		_identification = identification;
		_records        = records;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
		=> _register.Get(circuit).AsTask();

	public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_identification.Remove(_records.Get(circuit).Identifier);
		return base.OnCircuitClosedAsync(circuit, cancellationToken);
	}
}