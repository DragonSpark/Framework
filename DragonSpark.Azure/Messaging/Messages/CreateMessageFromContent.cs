using System;
using Azure.Messaging.ServiceBus;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Azure.Messaging.Messages;

sealed class CreateMessageFromContent : IParser<ServiceBusMessage>
{
	readonly TimeSpan?                                _life, _fromNow;
	readonly ISelect<MessageInput, ServiceBusMessage> _create;

	public CreateMessageFromContent(TimeSpan? life, TimeSpan? fromNow, ISelect<MessageInput, ServiceBusMessage> create)
	{
		_life    = life;
		_fromNow = fromNow;
		_create  = create;
	}

	public ServiceBusMessage Get(string parameter) => _create.Get(new(parameter, _fromNow, _life));
}