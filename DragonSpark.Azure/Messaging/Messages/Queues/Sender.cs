using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

public class Sender : ISender
{
	readonly ServiceBusSender                         _instance;
	readonly ISelect<MessageInput, ServiceBusMessage> _create;

	protected Sender(ServiceBusClient client, string name, ServiceBusConfiguration configuration)
		: this(client, name, configuration.Audience) {}

	protected Sender(ServiceBusClient client, string name, string? audience)
		: this(client.CreateSender($"{name}{audience}"), CreateMessage.Default) {}

	protected Sender(ServiceBusSender instance, ISelect<MessageInput, ServiceBusMessage> create)
	{
		_instance = instance;
		_create   = create;
	}

	public ISend Get(SendInput parameter)
	{
		var (life, visibility) = parameter;
		return new Send(_instance, new CreateMessageFromContent(life, visibility, _create));
	}

	public ValueTask Get(Stop<MessageInput> parameter)
	{
		var (subject, stop) = parameter;
		var message = _create.Get(subject);
		return _instance.SendMessageAsync(message, stop).ToOperation();
	}
}