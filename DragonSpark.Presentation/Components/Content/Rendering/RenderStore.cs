using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderStore<T> : MemoryVariable<T>
{
	public RenderStore(IMemoryCache memory, string key) : base(memory, key, RenderStoreConfiguration.Default) {}
}