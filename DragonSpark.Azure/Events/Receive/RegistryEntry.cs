using System;

namespace DragonSpark.Azure.Events.Receive;

public sealed record RegistryEntry(Type MessageType, Handlers Handlers)
{
	public RegistryEntry(Type type) : this(type, new()) {}
}