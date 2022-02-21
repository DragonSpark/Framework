using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Environment;

public sealed class ContextRenderInitialize : RenderInitialize<HttpContext>
{
	public ContextRenderInitialize(IMemoryCache memory, ContextStore store)
		: base(memory, ContextHostRenderKey.Default, store) {}
}