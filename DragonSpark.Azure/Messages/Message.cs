using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messages;

sealed class Message : IMessage
{
	readonly ServiceBusSender                         _client;
	readonly ISelect<MessageInput, ServiceBusMessage> _create;

	public Message(ServiceBusSender client) : this(client, CreateMessage.Default) {}

	public Message(ServiceBusSender client, ISelect<MessageInput, ServiceBusMessage> create)
	{
		_client = client;
		_create = create;
	}

	public ValueTask Get(MessageInput parameter)
	{
		var message = _create.Get(parameter);
		return _client.SendMessageAsync(message).ToOperation();
	}
}