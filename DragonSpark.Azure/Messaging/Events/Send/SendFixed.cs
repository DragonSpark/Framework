﻿using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Send;

public class SendFixed<T> : IOperation<uint> where T : Message, new()
{
	readonly IOperation<CreateEventDataInput> _previous;
	readonly T                                _message;

	protected SendFixed(IProducer client) : this(client, new T()) {}

	protected SendFixed(IProducer client, T message) : this(new SendTo<T>(client), message) {}

	protected SendFixed(IOperation<CreateEventDataInput> previous, T message)
	{
		_previous = previous;
		_message  = message;
	}

	public ValueTask Get(uint parameter) => _previous.Get(new(parameter, _message));
}