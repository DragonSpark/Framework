using DragonSpark.Application.Entities.Queries.Runtime.Shape;

namespace DragonSpark.Presentation.Components.Content.Sequences;

// TODO:

sealed class PreRenderAwarePagers<T> : IPagers<T>
{
	readonly IPagers<T> _previous;

	public PreRenderAwarePagers(IPagers<T> previous) => _previous = previous;

	public IPaging<T> Get(PagingInput<T> parameter) => _previous.Get(parameter);
}