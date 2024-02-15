using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Send;

public class SendTo : IOperation<CreateEventDataInput>
{
	readonly IProducer        _client;
	readonly ICreateEventData _create;

	protected SendTo(IProducer client, ICreateEventData create)
	{
		_client = client;
		_create = create;
	}

	public ValueTask Get(CreateEventDataInput parameter)
	{
		var data = _create.Get(parameter);
		return _client.Get(data);
	}
}

public sealed class SendTo<T> : SendTo
{
	public SendTo(IProducer client) : base(client, CreateEventData<T>.Default) {}
}

public class SendTo<T, U> : IOperation<T> where U : Message
{
	readonly IProducer      _client;
	readonly Func<T, ulong> _recipient;
	readonly Func<T, U>     _message;

	protected SendTo(IProducer client, Func<T, ulong> recipient, Func<T, U> message)
	{
		_client    = client;
		_recipient = recipient;
		_message   = message;
	}

	public ValueTask Get(T parameter)
	{
		var message   = _message(parameter);
		var recipient = _recipient(parameter);
		var data      = CreateEventData<U>.Default.Get(new(recipient.Contract(), message));
		return _client.Get(data);
	}
}