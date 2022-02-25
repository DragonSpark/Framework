using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class RecordAwareCircuitHandler : CircuitHandler
{
	readonly RegisterCircuit _register;
	readonly ClearCircuit    _clear;

	public RecordAwareCircuitHandler(RegisterCircuit register, ClearCircuit clear)
	{
		_register = register;
		_clear    = clear;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
		=> _register.Get(circuit).AsTask();

	public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
		=> _clear.Get(circuit).AsTask();
}