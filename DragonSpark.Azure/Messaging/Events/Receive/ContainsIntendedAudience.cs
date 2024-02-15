using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed class ContainsIntendedAudience : Condition<ProcessEventArgs>
{
	public ContainsIntendedAudience(string? audience)
		: base(new Messaging.Receive.ContainsIntendedAudience(audience).Get) {}
}