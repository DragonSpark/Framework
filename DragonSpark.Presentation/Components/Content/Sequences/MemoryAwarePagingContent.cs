using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class MemoryAwarePagingContent<T> : RenderStateContent<QueryInput, Current<T>>
{
	public MemoryAwarePagingContent(IPaging<T> previous, IMemoryCache memory) : base(previous, memory) {}
}