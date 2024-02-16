using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

sealed class Send : ISend
{
	readonly ServiceBusSender                   _sender;
	readonly ISelect<string, ServiceBusMessage> _message;

	public Send(ServiceBusSender sender, ISelect<string, ServiceBusMessage> message)
	{
		_sender  = sender;
		_message = message;
	}

	public ValueTask Get(string parameter)
	{
		var message = _message.Get(parameter);
		return _sender.SendMessageAsync(message).ToOperation();
	}
}