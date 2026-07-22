using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Senders : ConcurrentStore<string, ServiceBusSender>
{
	public Senders(ServiceBusClient client) : base(client.CreateSender) {}
}