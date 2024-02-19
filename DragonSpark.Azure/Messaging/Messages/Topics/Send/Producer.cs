using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public class Producer : IProducer
{
	readonly ServiceBusSender _instance;
	
	protected Producer(ServiceBusClient client, string name) : this(client.CreateSender(name)) {}

	public Producer(ServiceBusSender instance) => _instance = instance;

	public ValueTask Get(ServiceBusMessage parameter) => _instance.SendMessageAsync(parameter).ToOperation();
}