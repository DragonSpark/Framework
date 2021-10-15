using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class PreRenderAwareActiveContents<T> : IActiveContents<T>
{
	readonly IActiveContents<T>           _previous;
	readonly IsPreRendering               _condition;
	readonly MemoryAwareActiveContents<T> _memory;

	public PreRenderAwareActiveContents(IActiveContents<T> previous, IsPreRendering condition,
	                                    MemoryAwareActiveContents<T> memory)
	{
		_previous  = previous;
		_condition = condition;
		_memory    = memory;
	}

	public IActiveContent<T> Get(Func<ValueTask<T>> parameter)
	{
		var condition = _condition.Get();
		var content   = _previous.Get(parameter);
		var result = condition
			             ? new PreRenderActiveContent<T>(_condition, _memory.Get(parameter), content)
			             : content;
		return result;
	}
}