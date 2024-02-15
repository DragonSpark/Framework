using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages;

sealed class Send : ISend
{
	readonly ServiceBusSender                   _client;
	readonly ISelect<string, ServiceBusMessage> _message;

	public Send(ServiceBusSender client, TimeSpan? life, TimeSpan? fromNow)
		: this(client, new CreateMessageFromContent(life, fromNow)) {}

	public Send(ServiceBusSender client, ISelect<string, ServiceBusMessage> message)

	{
		_client  = client;
		_message = message;
	}

	public ValueTask Get(string parameter)
	{
		var message = _message.Get(parameter);
		return _client.SendMessageAsync(message).ToOperation();
	}
}