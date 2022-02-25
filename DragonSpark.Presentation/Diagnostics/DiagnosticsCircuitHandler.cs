using DragonSpark.Diagnostics.Logging;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Diagnostics;

sealed class DiagnosticsCircuitHandler : CircuitHandler
{
	readonly Connected _connected;
	readonly Closed    _closed;

	public DiagnosticsCircuitHandler(Connected connected, Closed closed)
	{
		_connected = connected;
		_closed    = closed;
	}

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_connected.Execute(circuit.Id);
		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}

	public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_closed.Execute(circuit.Id);
		return base.OnCircuitClosedAsync(circuit, cancellationToken);
	}

	public sealed class Connected : LogMessage<string>
	{
		public Connected(ILogger<Connected> logger) : base(logger, "Circuit {Identity} connected") {}
	}

	public sealed class Closed : LogMessage<string>
	{
		public Closed(ILogger<Closed> logger) : base(logger, "Circuit {Identity} closed") {}
	}
}