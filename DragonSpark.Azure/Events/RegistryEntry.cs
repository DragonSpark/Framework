using System;

namespace DragonSpark.Azure.Events;

public sealed record RegistryEntry(Type Key, Handlers Handlers)
{
	public RegistryEntry(Type Key) : this(Key, new()) {}
}