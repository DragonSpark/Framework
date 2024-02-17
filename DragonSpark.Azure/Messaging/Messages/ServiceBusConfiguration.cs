using Azure.Messaging.ServiceBus;

namespace DragonSpark.Azure.Messaging.Messages;

public sealed class ServiceBusConfiguration : MessagingConfiguration
{
	public ServiceBusTransportType TransportType { get; set; } = ServiceBusTransportType.AmqpWebSockets;

	public string Subscription { get; set; } = "Default";
}