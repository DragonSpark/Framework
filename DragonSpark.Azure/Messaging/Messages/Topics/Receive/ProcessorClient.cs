using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Data;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public class ProcessorClient : Instance<ServiceBusProcessor>
{
	protected ProcessorClient(ServiceBusConfiguration configuration, string topic, string subscription = "Default")
		: this(new ServiceBusClient($"{configuration.Namespace}.servicebus.windows.net", DefaultCredential.Default),
		       topic, subscription) {}

	public ProcessorClient(ServiceBusClient client, string topic, string subscription)
		: base(client.CreateProcessor(topic, subscription)) {}
}