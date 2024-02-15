using System;

namespace DragonSpark.Azure.Messaging.Events.Receive;

public sealed record RegistryEntry(Type MessageType, Handlers Handlers)
{
	public RegistryEntry(Type type) : this(type, new()) {}
}