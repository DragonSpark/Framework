using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public class Send<T, U> : IStopAware<T> where U : Message
{
	readonly IProducer        _producer;
	readonly Func<T, U>       _select;
	readonly ICreateEventData _create;

	protected Send(IProducer producer, Func<T, U> select) : this(producer, select, CreateEventData<U>.Default) {}

	protected Send(IProducer producer, Func<T, U> select, ICreateEventData create)
	{
		_producer = producer;
		_select   = select;
		_create   = create;
	}

	public ValueTask Get(Stop<T> parameter)
	{
		var message = _select(parameter);
		var data    = _create.Get(new(message));
		return _producer.Get(new(data, parameter));
	}
}