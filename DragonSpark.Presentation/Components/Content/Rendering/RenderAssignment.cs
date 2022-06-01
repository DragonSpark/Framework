using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderAssignment<T> : MemoryAssignment<T>
{
	public RenderAssignment(IMemoryCache subject) : base(subject, RenderStoreConfiguration.Default) {}
}