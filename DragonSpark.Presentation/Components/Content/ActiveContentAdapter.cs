using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContentAdapter<T> : IActiveContent<T>
{
	readonly IResulting<T?> _previous;

	public ActiveContentAdapter(IOperation<Action> refresh, IResulting<T?> previous)
	{
		Refresh   = refresh;
		_previous = previous;
	}

	public IOperation<Action> Refresh { get; }

	public ValueTask<T?> Get() => _previous.Get();
}