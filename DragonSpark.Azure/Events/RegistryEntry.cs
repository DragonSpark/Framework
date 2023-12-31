using DragonSpark.Model.Operations;
using System;
using System.Collections.Concurrent;

namespace DragonSpark.Azure.Events;

public sealed record RegistryEntry(Type Key, ConcurrentBag<IOperation<object>> Handlers)
{
	public RegistryEntry(Type Key) : this(Key, new()) {}
}