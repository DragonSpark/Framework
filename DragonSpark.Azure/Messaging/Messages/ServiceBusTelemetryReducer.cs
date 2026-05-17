using OpenTelemetry;
using System.Diagnostics;

namespace DragonSpark.Azure.Messaging.Messages;

public sealed class ServiceBusTelemetryReducer : BaseProcessor<Activity>
{
	public static ServiceBusTelemetryReducer Default { get; } = new();

	ServiceBusTelemetryReducer() {}

	public override void OnEnd(Activity activity)
	{
		var enable = activity.Source.Name == "Azure.Messaging.ServiceBus" ||
		             activity.GetTagItem("az.namespace") is "Microsoft.ServiceBus";
		if (enable && activity.OperationName == "ServiceBusReceiver.Receive")
		{
			activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
		}
	}
}