using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Environment;

sealed class ContextRenderInitialize : RenderInitialize<HttpContext>
{
	public ContextRenderInitialize(IMemoryCache memory, ContextHostRenderKey key, ContextStore store)
		: base(memory, key, store) {}
}