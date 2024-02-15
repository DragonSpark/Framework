using Azure.Messaging.ServiceBus;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages;

sealed class Send : ISend
{
	readonly ISender                            _client;
	readonly ISelect<string, ServiceBusMessage> _message;

	public Send(ISender client, ISelect<string, ServiceBusMessage> message)
	{
		_client  = client;
		_message = message;
	}

	public ValueTask Get(string parameter)
	{
		var message = _message.Get(parameter);
		return _client.Get(message);
	}
}