using Azure.Messaging.ServiceBus;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Send;

public class CreateEventData : ICreateEventData
{
	readonly string _key;

	protected CreateEventData(string key) => _key = key;

	public ServiceBusMessage Get(CreateEventDataInput parameter)
	{
		var (recipient, message) = parameter;
		var body   = BinaryData.FromObjectAsJson(message);
		var result = new ServiceBusMessage(body) { ApplicationProperties = { [EventType.Default] = _key } };

		if (recipient is not null)
		{
			result.ApplicationProperties[Recipient.Default] = recipient.Value;
		}

		return result;
	}
}

public sealed class CreateEventData<T> : CreateEventData
{
	public static CreateEventData<T> Default { get; } = new();

	CreateEventData() : base(A.Type<T>().FullName.Verify()) {}
}