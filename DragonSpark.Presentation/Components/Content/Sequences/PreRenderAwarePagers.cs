using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderAwarePagers<T> : IPagers<T>
{
	readonly PreRenderingAwarePagerBuilder<T> _builder;
	readonly IFormatter<QueryInput>           _key;

	public PreRenderAwarePagers(PreRenderingAwarePagerBuilder<T> builder, IFormatter<QueryInput> key)
	{
		_builder = builder;
		_key     = key;
	}

	public IPaging<T> Get(PagingInput<T> parameter) => _builder.Get(new(parameter, _key));
}

// TODO

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