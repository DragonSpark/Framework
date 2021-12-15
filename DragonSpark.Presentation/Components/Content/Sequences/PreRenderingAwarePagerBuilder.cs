using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderingAwarePagerBuilder<T> : ISelect<PagersInput<T>, IPaging<T>>
{
	readonly IPagers<T>                 _previous;
	readonly IsPreRendering             _condition;
	readonly PreRenderingAwarePagers<T> _pagers;

	public PreRenderingAwarePagerBuilder(IsPreRendering condition, PreRenderingAwarePagers<T> pagers)
		: this(Pagers<T>.Default, condition, pagers) {}

	public PreRenderingAwarePagerBuilder(IPagers<T> previous, IsPreRendering condition,
	                                     PreRenderingAwarePagers<T> pagers)
	{
		_previous  = previous;
		_condition = condition;
		_pagers    = pagers;
	}

	public IPaging<T> Get(PagersInput<T> parameter)
	{
		var (input, formatter) = parameter;
		var previous = _previous.Get(input);
		var first    = _condition.Get();
		var result = first
			             ? new PreRenderAwarePaging<T>(_condition, _pagers.Get(new(previous, formatter)), previous)
			             : previous;
		return result;
	}
}