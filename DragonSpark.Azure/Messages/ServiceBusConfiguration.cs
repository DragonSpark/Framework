using Azure.Messaging.ServiceBus;

namespace DragonSpark.Azure.Messages;

public sealed class ServiceBusConfiguration
{
	public string Namespace { get; set; } = default!;

	public ServiceBusTransportType TransportType { get; set; } = ServiceBusTransportType.AmqpWebSockets;
}