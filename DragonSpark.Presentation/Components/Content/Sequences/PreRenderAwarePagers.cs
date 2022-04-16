using DragonSpark.Application.Entities.Queries.Runtime.Shape;
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