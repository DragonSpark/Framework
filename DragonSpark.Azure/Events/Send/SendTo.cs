using Azure.Messaging.EventHubs.Producer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Send;

public class SendTo : IOperation<CreateEventDataInput>
{
	readonly EventHubProducerClient _client;
	readonly ICreateEventData       _create;

	public SendTo(EventHubProducerClient client, ICreateEventData create)
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