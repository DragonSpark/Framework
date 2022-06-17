using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderVariable<T> : MemoryVariable<T>
{
	public RenderVariable(IMemoryCache memory, string key) : base(memory, key, RenderStoreConfiguration.Default) {}
}