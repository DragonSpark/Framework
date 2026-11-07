using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DragonSpark.Sentry;

public sealed class SentryCircuitHandler : CircuitHandler
{
	readonly IHub _hub;

	public SentryCircuitHandler(IHub hub) => _hub = hub;

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_hub.ConfigureScope(static (scope, circuitId) => scope.SetTag("CircuitId", circuitId), circuit.Id);

		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}
}