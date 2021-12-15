using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

internal class Class1 {}

sealed class SingletonAwareActiveContent<T> : Deferring<T?>, IActiveContent<T>
{
	public SingletonAwareActiveContent(IActiveContent<T> previous) : base(previous.ToDelegate()) {}
}

sealed class SingletonAwareActiveContents<T> : IActiveContents<T>
{
	readonly IActiveContents<T> _previous;

	public SingletonAwareActiveContents(IActiveContents<T> previous) => _previous = previous;

	public IActiveContent<T> Get(Func<ValueTask<T?>> parameter)
		=> new SingletonAwareActiveContent<T>(_previous.Get(parameter));
}