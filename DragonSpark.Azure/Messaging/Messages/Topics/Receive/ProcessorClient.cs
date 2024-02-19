using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Data;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class ProcessorClient : Instance<ServiceBusProcessor>
{
	protected ProcessorClient(ServiceBusConfiguration configuration, string topic)
		: this(new ServiceBusClient($"{configuration.Namespace}.servicebus.windows.net", DefaultCredential.Default),
		       topic, $"{configuration.Subscription}{configuration.Audience}") {}

	public ProcessorClient(ServiceBusClient client, string topic, string subscription)
		: base(client.CreateProcessor(topic, subscription)) {}
}