using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public class Producer : IProducer
{
	readonly ServiceBusSender _instance;
	readonly string?          _audience;

	protected Producer(ServiceBusClient client, string name, ServiceBusConfiguration settings)
		: this(client.CreateSender(name), settings.Audience) {}

	public Producer(ServiceBusSender instance, string? audience)
	{
		_instance = instance;
		_audience = audience;
	}

	public ValueTask Get(ServiceBusMessage parameter)
	{
		if (_audience is not null)
		{
			parameter.ApplicationProperties[IntendedAudience.Default] = _audience;
		}

		return _instance.SendMessageAsync(parameter).ToOperation();
	}
}