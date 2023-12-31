using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public class Send<T, U> : IOperation<T>
{
	readonly EventHubProducerClient _client;
	readonly Func<T, U>             _select;

	protected Send(EventHubProducerClient client, Func<T, U> select)
	{
		_client = client;
		_select = select;
	}

	public ValueTask Get(T parameter)
	{
		var message = _select(parameter);
		var data = new EventData(BinaryData.FromObjectAsJson(message))
		{
			Properties = { [EventType.Default] = A.Type<U>().FullName }
		}.Yield();
		return _client.SendAsync(data).ToOperation();
	}
}

public class Send<T> : IOperation<T>
{
	readonly EventHubProducerClient _client;

	protected Send(EventHubProducerClient client) => _client = client;

	public ValueTask Get(T parameter)
	{
		var data = new EventData(BinaryData.FromObjectAsJson(parameter))
		{
			Properties = { [EventType.Default] = A.Type<T>().FullName }
		}.Yield();
		return _client.SendAsync(data).ToOperation();
	}
}