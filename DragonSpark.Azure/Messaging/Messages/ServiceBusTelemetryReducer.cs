using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace DragonSpark.Azure.Messaging.Messages;

// Source - https://stackoverflow.com/a/76180024
// Posted by Peter Bons
// Retrieved 2026-02-14, License - CC BY-SA 4.0
public sealed class ServiceBusTelemetryReducer : ITelemetryProcessor
{
	private readonly ITelemetryProcessor _next;

	public ServiceBusTelemetryReducer(ITelemetryProcessor next) => _next = next;

	public void Process(ITelemetry item)
	{
		var process = item is not DependencyTelemetry { Type: "Azure Service Bus", Name: "ServiceBusReceiver.Receive" };

		if (process)
		{
			_next.Process(item);
		}
	}
}