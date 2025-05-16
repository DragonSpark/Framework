using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public sealed class StopAwareAdapter<T> : IStopAware<T>
{
	readonly IResulting<T> _previous;

	public StopAwareAdapter(IResulting<T> previous) => _previous = previous;

	public ValueTask<T> Get(CancellationToken parameter) => _previous.Get();
}