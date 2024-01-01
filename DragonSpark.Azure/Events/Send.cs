using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public class Send<T, U> : IOperation<T> where U : class
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

// TODO

public readonly record struct CreateEventDataInput(uint? Recipient, object Message)
{
	public CreateEventDataInput(object Message) : this(null, Message) {}
}

public interface ICreateEventData : ISelect<CreateEventDataInput, EventData>;

public sealed class CreateEventData<T> : CreateEventData
{
	public static CreateEventData<T> Default { get; } = new();

	CreateEventData() : base(A.Type<T>().FullName.Verify()) {}
}

public class CreateEventData : ICreateEventData
{
	readonly string _key;

	protected CreateEventData(string key) => _key = key;

	public EventData Get(CreateEventDataInput parameter)
	{
		var (recipient, message) = parameter;
		var result = new EventData(BinaryData.FromObjectAsJson(message))
		{
			Properties =
			{
				[EventType.Default] = _key
			}
		};

		if (recipient is not null)
		{
			result.Properties[Recipient.Default] = recipient.Value;
		}

		return result;
	}
}

public class SendTo<T> : IOperation<CreateEventDataInput>
{
	readonly EventHubProducerClient _client;
	readonly ICreateEventData       _create;

	protected SendTo(EventHubProducerClient client) : this(client, CreateEventData<T>.Default) {}

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