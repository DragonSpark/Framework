using DragonSpark.Compose;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store.Operations.Memory;

public readonly record struct StoreProfile<T>(IMemoryCache Memory, TimeSpan For, Func<T, string> Key)
{
	public StoreProfile(IMemoryCache Memory, TimeSpan For, string key) : this(Memory, For, key.Accept) {}
}