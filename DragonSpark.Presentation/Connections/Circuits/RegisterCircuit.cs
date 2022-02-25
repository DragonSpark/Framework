using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class RegisterCircuit : IOperation<Circuit>
{
	readonly CreateCircuitRecord            _create;
	readonly ITable<Circuit, CircuitRecord> _records;
	readonly IMutable<CircuitRecord?>       _context;

	public RegisterCircuit(CreateCircuitRecord create)
		: this(create, CircuitRecordStore.Default, CurrentCircuit.Default) {}

	public RegisterCircuit(CreateCircuitRecord create, ITable<Circuit, CircuitRecord> records,
	                       IMutable<CircuitRecord?> context)
	{
		_create  = create;
		_records = records;
		_context = context;
	}

	public async ValueTask Get(Circuit parameter)
	{
		var record = await _create.Await(parameter);
		_records.Assign(parameter, record);
		_context.Execute(record);
	}
}