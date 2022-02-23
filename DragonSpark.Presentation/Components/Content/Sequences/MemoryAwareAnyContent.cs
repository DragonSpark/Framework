using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class MemoryAwareAnyContent<T> : RenderStateContent<IQueries<T>, bool>
{
	public MemoryAwareAnyContent(IDepending<IQueries<T>> previous, IMemoryCache memory) : base(previous, memory) {}
}