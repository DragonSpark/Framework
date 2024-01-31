using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Send;

public class SendTo : IOperation<CreateEventDataInput>
{
	readonly EventHubProducerClient _client;
	readonly ICreateEventData       _create;

	protected SendTo(EventHubProducerClient client, ICreateEventData create)
	{
		_client = client;
		_create = create;
	}

	public ValueTask Get(CreateEventDataInput parameter)
	{
		var data = _create.Get(parameter).Yield();
		return _client.SendAsync(data).ToOperation();
	}
}

public sealed class SendTo<T> : SendTo
{
	public SendTo(EventHubProducerClient client) : base(client, CreateEventData<T>.Default) {}
}

public class SendTo<T, U> : IOperation<T> where U : Message
{
	readonly EventHubProducerClient _client;
	readonly Func<T, ulong>         _recipient;
	readonly Func<T, U>             _message;

	protected SendTo(EventHubProducerClient client, Func<T, ulong> recipient, Func<T, U> message)
	{
		_client         = client;
		_recipient = recipient;
		_message        = message;
	}

	public ValueTask Get(T parameter)
	{
		var message   = _message(parameter);
		var recipient = _recipient(parameter);
		var data      = CreateEventData<U>.Default.Get(new(recipient.Contract(), message)).Yield();
		return _client.SendAsync(data).ToOperation();
	}
}