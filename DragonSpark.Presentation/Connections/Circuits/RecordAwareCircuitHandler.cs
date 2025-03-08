using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class RecordAwareCircuitHandler : CircuitHandler
{
	readonly CreateCircuitRecord      _create;
	readonly CurrentCircuitStore      _store;
	readonly IMutable<CircuitRecord?> _current;

	public RecordAwareCircuitHandler(CreateCircuitRecord create, CurrentCircuitStore store)
		: this(create, store, CurrentCircuitRecord.Default) {}

	public RecordAwareCircuitHandler(CreateCircuitRecord create, CurrentCircuitStore store,
	                                 IMutable<CircuitRecord?> current)
	{
		_create  = create;
		_store   = store;
		_current = current;
	}

	public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		var record = await _create.Off(circuit);
		_store.Execute(record);
		_current.Execute(record);
	}
}