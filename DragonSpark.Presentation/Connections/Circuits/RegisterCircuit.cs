using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class RegisterCircuit : IOperation<Circuit>
{
	readonly CreateCircuitRecord            _create;
	readonly ITable<string, CircuitRecord>  _identification;
	readonly ITable<Circuit, CircuitRecord> _records;

	public RegisterCircuit(CreateCircuitRecord create)
		: this(create, CircuitRecordIdentification.Default, CircuitRecords.Default) {}

	public RegisterCircuit(CreateCircuitRecord create, ITable<string, CircuitRecord> identification,
	                       ITable<Circuit, CircuitRecord> records)
	{
		_create         = create;
		_identification = identification;
		_records        = records;
	}

	public async ValueTask Get(Circuit parameter)
	{
		var record = await _create.Await(parameter);
		_identification.Assign(record.Identifier, record);
		_records.Assign(parameter, record);
	}
}