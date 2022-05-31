using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContentAdapter<T> : IActiveContent<T>
{
	readonly IResulting<T?> _previous;

	public ActiveContentAdapter(IUpdateMonitor refresh, IResulting<T?> previous)
	{
		Monitor   = refresh;
		_previous = previous;
	}

	public IUpdateMonitor Monitor { get; }

	public ValueTask<T?> Get() => _previous.Get();
}