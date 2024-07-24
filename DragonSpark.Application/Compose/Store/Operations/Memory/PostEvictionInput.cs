using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store.Operations.Memory;

public readonly record struct PostEvictionInput(object Key, object? Value, EvictionReason Reason, object? State);