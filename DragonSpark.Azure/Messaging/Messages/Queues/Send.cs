using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Text;

namespace DragonSpark.Azure.Messaging.Messages.Queues;

sealed class Send : ISend
{
	readonly ServiceBusSender           _sender;
	readonly IParser<ServiceBusMessage> _message;

	public Send(ServiceBusSender sender, IParser<ServiceBusMessage> message)
	{
		_sender  = sender;
		_message = message;
	}

	public ValueTask Get(Stop<string> parameter)
	{
		var message = _message.Get(parameter);
		return _sender.SendMessageAsync(message, parameter).ToOperation();
	}
}