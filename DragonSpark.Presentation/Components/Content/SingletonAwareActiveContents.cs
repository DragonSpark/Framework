using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class SingletonAwareActiveContents<T> : IActiveContents<T>
{
	readonly IActiveContents<T> _previous;

	public SingletonAwareActiveContents(IActiveContents<T> previous) => _previous = previous;

	public IActiveContent<T> Get(Func<ValueTask<T?>> parameter)
		=> new SingletonAwareActiveContent<T>(_previous.Get(parameter));
}