using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

sealed class StopAwareOperationAdapter<T> : IOperation<T>
{
	readonly ISelect<Stop<T>, ValueTask> _previous;

	public StopAwareOperationAdapter(ISelect<Stop<T>, ValueTask> previous) => _previous = previous;

	public ValueTask Get(T parameter) => _previous.Get(new(parameter, CancellationToken.None)); // TODO
}