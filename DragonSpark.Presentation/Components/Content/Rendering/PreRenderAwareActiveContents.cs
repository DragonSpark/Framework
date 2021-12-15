using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class PreRenderAwareActiveContents<T> : IActiveContents<T>
{
	readonly IActiveContents<T>                 _previous;
	readonly IsPreRendering                     _condition;
	readonly RenderAwareActiveContentBuilder<T> _builder;

	public PreRenderAwareActiveContents(IActiveContents<T> previous, IsPreRendering condition,
	                                    RenderAwareActiveContentBuilder<T> builder)
	{
		_previous  = previous;
		_condition = condition;
		_builder   = builder;
	}

	public IActiveContent<T> Get(Func<ValueTask<T?>> parameter)
	{
		var condition = _condition.Get();
		var content   = _previous.Get(parameter);
		var result = condition
			             ? new PreRenderActiveContent<T>(_condition, _builder.Get(parameter), content)
			             : content;
		return result;
	}
}