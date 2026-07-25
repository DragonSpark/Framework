using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class SendMessage : ISendMessage
{
	readonly Senders                                       _senders;
	readonly ISelect<MessageProperties, ServiceBusMessage> _create;

	public SendMessage(Senders senders) : this(senders, ComposeMessage.Default) {}

	public SendMessage(Senders senders, ISelect<MessageProperties, ServiceBusMessage> create)
	{
		_senders = senders;
		_create  = create;
	}

	public ValueTask Get(Stop<DurableMessageProperties> parameter)
	{
		var ((identifier, message, destination, visibility, life), stop) = parameter;
		var input  = _create.Get(new(new(message, identifier), visibility, life));
		var sender = _senders.Get(destination);
		return sender.SendMessageAsync(input, stop).ToOperation();
	}
}