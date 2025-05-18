using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

sealed class StopAwareAdapter<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelect<Stop<TIn>, ValueTask<TOut>> _previous;

	public StopAwareAdapter(ISelect<Stop<TIn>, ValueTask<TOut>> previous) => _previous = previous;

	public ValueTask<TOut> Get(TIn parameter) => _previous.Get(new(parameter));
}