using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class IdentifiedPagings<T> : ISelect<object, Pagings<T>>
{
	readonly IRenderContentKey                _key;
	readonly PreRenderingAwarePagerBuilder<T> _builder;
	readonly PreRenderingAwareAnyBuilder<T>   _any;

	public IdentifiedPagings(IRenderContentKey key, PreRenderingAwarePagerBuilder<T> builder,
	                         PreRenderingAwareAnyBuilder<T> any)
	{
		_key     = key;
		_builder = builder;
		_any     = any;
	}

	public Pagings<T> Get(object parameter)
	{
		var key       = _key.Get(parameter);
		var formatter = new QueryInputKey(key);
		var pagers    = new PreRenderAwarePagers<T>(_builder, formatter);
		var any       = _any.Get(key);
		return new (pagers, any);
	}
}