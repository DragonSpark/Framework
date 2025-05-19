using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

sealed class StopAwareAdapter<T> : IStopAware<T>
{
	readonly ISelect<T, ValueTask> _previous;

	public StopAwareAdapter(ISelect<T, ValueTask> previous) => _previous = previous;

	public ValueTask Get(Stop<T> parameter) => _previous.Get(parameter.Subject);
}