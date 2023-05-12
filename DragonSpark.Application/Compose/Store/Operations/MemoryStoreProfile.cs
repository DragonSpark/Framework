using DragonSpark.Compose;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store.Operations;

public readonly record struct MemoryStoreProfile<T>(IMemoryCache Memory, TimeSpan For, Func<T, string> Key)
{
	public MemoryStoreProfile(IMemoryCache Memory, TimeSpan For, string key) : this(Memory, For, key.Accept) {}
}