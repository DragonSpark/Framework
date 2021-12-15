using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderAwarePagers<T> : IPagers<T>
{
	readonly PreRenderingAwarePagerBuilder<T> _pagers;
	readonly IFormatter<QueryInput>           _key;

	public PreRenderAwarePagers(PreRenderingAwarePagerBuilder<T> pagers, IFormatter<QueryInput> key)
	{
		_pagers = pagers;
		_key    = key;
	}

	public IPaging<T> Get(PagingInput<T> parameter) => _pagers.Get(new(parameter, _key));
}