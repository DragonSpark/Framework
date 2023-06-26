using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class ComposerAwareAny<T> : IAny<T>
{
	readonly IAny<T> _previous;

	public ComposerAwareAny(IAny<T> previous) => _previous = previous;

	public ValueTask<bool> Get(AnyInput<T> parameter)
	{
		var (owner, _) = parameter;
		var any    = (owner is IAnyComposer<T> @delegate ? @delegate.Get(_previous) : null) ?? _previous;
		var result = any.Get(parameter);
		return result;
	}
}