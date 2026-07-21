using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Senders : ReferenceValueStore<string, ServiceBusSender>
{
	public Senders(ServiceBusClient client)
		: base(Start.A.Selection<string>().By.Calling(string.Intern).Select(client.CreateSender)) {}
}