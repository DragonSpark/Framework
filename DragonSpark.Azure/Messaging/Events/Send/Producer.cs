using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Azure.Data;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Send;

public class Producer : IProducer
{
	readonly EventHubProducerClient _instance;
	readonly string?                _audience;

	protected Producer(EventHubConfiguration settings, string name)
		: this(new EventHubProducerClient($"{settings.Namespace}.servicebus.windows.net", name, DefaultCredential.Default),
		       settings.Audience) {}

	public Producer(EventHubProducerClient instance, string? audience)
	{
		_instance = instance;
		_audience = audience;
	}

	public ValueTask Get(EventData parameter)
	{
		if (_audience is not null)
		{
			parameter.Properties[IntendedAudience.Default] = _audience;
		}

		return _instance.SendAsync(parameter.Yield()).ToOperation();
	}
}