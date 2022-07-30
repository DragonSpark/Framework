using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Environment;

sealed class ContextCache : MemoryCache
{
	public ContextCache() : base(new MemoryCacheOptions()) {}
}