using Azure.Core;
using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Azure.Data;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Events;

public class Producer : Instance<EventHubProducerClient>, IProducer
{
	protected Producer(EventHubConfiguration settings, string name)
		: this(settings.Namespace, name, DefaultCredential.Default) {}

	protected Producer(string @namespace, string name, TokenCredential credential)
		: base(new($"{@namespace}.servicebus.windows.net", name, credential)) {}
}