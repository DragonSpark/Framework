using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderCache : MemoryCache
{
	public RenderCache() : base(new MemoryCacheOptions()) {}
}