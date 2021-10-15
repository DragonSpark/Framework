using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContentAdapter<T> : IActiveContent<T>
{
	readonly IResulting<T?> _previous;

	public ActiveContentAdapter(IResulting<T?> previous) => _previous = previous;

	public ValueTask<T?> Get() => _previous.Get();
}