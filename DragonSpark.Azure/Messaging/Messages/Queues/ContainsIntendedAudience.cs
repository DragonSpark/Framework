using Azure.Messaging.ServiceBus;
using DragonSpark.Compose.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public sealed class ContainsIntendedAudience : Condition<ServiceBusReceivedMessage>
{
	public ContainsIntendedAudience(string? audience) : base(new Receive.ContainsIntendedAudience(audience).Get) {}
}