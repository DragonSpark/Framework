﻿using Azure.Messaging.EventHubs;
using DragonSpark.Compose;
using System;

namespace DragonSpark.Azure.Events.Send;

public class CreateEventData : ICreateEventData
{
	readonly string _key;

	protected CreateEventData(string key) => _key = key;

	public EventData Get(CreateEventDataInput parameter)
	{
		var (recipient, message) = parameter;
		var body   = BinaryData.FromObjectAsJson(message);
		var result = new EventData(body) { Properties = { [EventType.Default] = _key } };

		if (recipient is not null)
		{
			result.Properties[Recipient.Default] = recipient.Value;
		}

		return result;
	}
}

public sealed class CreateEventData<T> : CreateEventData
{
	public static CreateEventData<T> Default { get; } = new();

	CreateEventData() : base(A.Type<T>().FullName.Verify()) {}
}