using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderingAwareAny<T> : ISelect<PreRenderingAwareAnyInput<T>, IDepending<IQueries<T>>>
{
	readonly IMemoryCache _memory;
	readonly RenderStates _states;

	public PreRenderingAwareAny(IMemoryCache memory, RenderStates states)
	{
		_memory = memory;
		_states = states;
	}

	public IDepending<IQueries<T>> Get(PreRenderingAwareAnyInput<T> parameter)
	{
		var (previous, key) = parameter;
		var content = new MemoryAwareAnyContent<T>(previous, _memory);
		var result  = new MemoryAwareAny<T>(key, _states, content);
		return result;
	}
}