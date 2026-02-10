using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Stop;

sealed class StopAwareAdapter<T> : IStopAware<T>
{
	readonly ISelect<T, ValueTask> _previous;

	public StopAwareAdapter(ISelect<T, ValueTask> previous) => _previous = previous;

	public ValueTask Get(Stop<T> parameter) => _previous.Get(parameter.Subject);
}

sealed class StopAwareAdapter : IStopAware
{
    readonly IResult<ValueTask> _previous;

    public StopAwareAdapter(IResult<ValueTask> previous) => _previous = previous;

    public ValueTask Get(CancellationToken parameter) => _previous.Get();
}