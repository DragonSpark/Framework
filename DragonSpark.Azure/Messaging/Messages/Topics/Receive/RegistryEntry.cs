using System;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed record RegistryEntry(Type MessageType, Handlers Handlers)
{
	public RegistryEntry(Type type) : this(type, new()) {}
}