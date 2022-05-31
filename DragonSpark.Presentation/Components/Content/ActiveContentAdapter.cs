using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContentAdapter<T> : IActiveContent<T>
{
	readonly IResulting<T?> _previous;

	public ActiveContentAdapter(IRequiresUpdate refresh, IResulting<T?> previous)
	{
		Refresh   = refresh;
		_previous = previous;
	}

	public IRequiresUpdate Refresh { get; }

	public ValueTask<T?> Get() => _previous.Get();
}