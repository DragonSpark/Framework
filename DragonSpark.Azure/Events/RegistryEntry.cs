using System;

namespace DragonSpark.Azure.Events;

public sealed record RegistryEntry(Type MessageType, Handlers Handlers)
{
	public RegistryEntry(Type type) : this(type, new()) {}
}