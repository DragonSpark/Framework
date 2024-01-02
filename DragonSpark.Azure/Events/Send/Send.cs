using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Send;

public class Send<T, U> : IOperation<T> where U : Message
{
	readonly EventHubProducerClient _client;
	readonly Func<T, U>             _select;
	readonly ICreateEventData       _create;

	protected Send(EventHubProducerClient client, Func<T, U> select)
		: this(client, select, CreateEventData<U>.Default) {}

	protected Send(EventHubProducerClient client, Func<T, U> select, ICreateEventData create)
	{
		_client = client;
		_select = select;
		_create = create;
	}

	public ValueTask Get(T parameter)
	{
		var message = _select(parameter);
		var data    = _create.Get(new(message)).Yield();
		return _client.SendAsync(data).ToOperation();
	}
}