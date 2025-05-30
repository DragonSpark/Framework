using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public class Peek : IPeek
{
	readonly ServiceBusReceiver _receiver;

	protected Peek(ServiceBusClient client, string name, ServiceBusConfiguration configuration)
		: this(client, name, configuration.Audience) {}

	protected Peek(ServiceBusClient client, string name, string? audience)
		: this(client.CreateReceiver($"{name}{audience}")) {}


	public Peek(ServiceBusReceiver receiver) => _receiver = receiver;

	public ValueTask<ServiceBusReceivedMessage?> Get() => _receiver.PeekMessageAsync().ToOperation();
}