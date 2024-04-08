using DragonSpark.Compose;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

public readonly record struct StoreProfile<T>(IDistributedCache Memory, TimeSpan For, Func<T, string> Key)
{
	public StoreProfile(IDistributedCache Memory, TimeSpan For, string key) : this(Memory, For, key.Accept) {}
}