using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderingAwarePagers<T> : ISelect<PreRenderingAwarePagersInput<T>, IPaging<T>>
{
	readonly IMemoryCache _memory;
	readonly RenderStates _states;

	public PreRenderingAwarePagers(IMemoryCache memory, RenderStates states)
	{
		_memory = memory;
		_states = states;
	}

	public IPaging<T> Get(PreRenderingAwarePagersInput<T> parameter)
	{
		var (previous, formatter) = parameter;
		var content = new MemoryAwarePagingContent<T>(previous, _memory);
		var result  = new MemoryAwarePaging<T>(formatter, _states, content);
		return result;
	}
}