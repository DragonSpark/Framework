using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public class SendTo : IStopAware<CreateEventDataInput>
{
	readonly IProducer        _client;
	readonly ICreateEventData _create;

	protected SendTo(IProducer client, ICreateEventData create)
	{
		_client = client;
		_create = create;
	}

	public ValueTask Get(Stop<CreateEventDataInput> parameter)
	{
		var (_, stop) = parameter;
		var data = _create.Get(parameter);
		return _client.Get(new(data, stop));
	}
}

public sealed class SendTo<T> : SendTo
{
	public SendTo(IProducer client) : base(client, CreateEventData<T>.Default) {}
}

public class SendTo<T, U> : IStopAware<T> where U : Message
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

	public ValueTask Get(Stop<T> parameter)
	{
		var (_, stop) = parameter;
		var message   = _message(parameter);
		var recipient = _recipient(parameter);
		var data      = CreateEventData<U>.Default.Get(new(recipient.Contract(), message));
		return _client.Get(new(data, stop));
	}
}