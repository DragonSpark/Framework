using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Azure.Data;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Send;

public class Producer : IProducer
{
	readonly EventHubProducerClient _instance;

	protected Producer(EventHubConfiguration settings, string name)
		: this(new EventHubProducerClient($"{settings.Namespace}.servicebus.windows.net", name,
		                                  DefaultCredential.Default)) {}

	public Producer(EventHubProducerClient instance) => _instance = instance;

	public ValueTask Get(EventData parameter) => _instance.SendAsync(parameter.Yield()).ToOperation();
}