using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results.Stop;

public sealed class StopAwareAdapter<T> : IStopAware<T>
{
	readonly IResulting<T> _previous;

	public StopAwareAdapter(IResulting<T> previous) => _previous = previous;

	public ValueTask<T> Get(CancellationToken parameter) => _previous.Get();
}