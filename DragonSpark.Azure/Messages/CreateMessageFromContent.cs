using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Azure.Messages;

sealed class CreateMessageFromContent : ISelect<string, ServiceBusMessage>
{
	readonly TimeSpan?                                _life, _fromNow;
	readonly ISelect<MessageInput, ServiceBusMessage> _create;

	public CreateMessageFromContent(TimeSpan? life, TimeSpan? fromNow) : this(life, fromNow, CreateMessage.Default) {}

	public CreateMessageFromContent(TimeSpan? life, TimeSpan? fromNow, ISelect<MessageInput, ServiceBusMessage> create)
	{
		_life    = life;
		_fromNow = fromNow;
		_create  = create;
	}

	public ServiceBusMessage Get(string parameter) => _create.Get(new(parameter, _fromNow, _life));
}