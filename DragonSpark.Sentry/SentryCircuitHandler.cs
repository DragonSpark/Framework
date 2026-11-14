using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DragonSpark.Sentry;

public sealed class SentryCircuitHandler : CircuitHandler
{
	readonly IHub _hub;

	public SentryCircuitHandler(IHub hub) => _hub = hub;

	public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
		Func<CircuitInboundActivityContext, Task> next)
		=> async context =>
		   {
			   using (_hub.PushScope())
			   {
				   _hub.ConfigureScope(static (scope, circuitId) => scope.SetTag("CircuitId", circuitId),
				                       context.Circuit.Id);

				   await next(context);
			   }
		   };
}