using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

sealed class ContinuingAdapter<TIn, TOut> : IContinuing<TIn, TOut>
{
	readonly ISelect<TIn, ValueTask<TOut>> _previous;

	public ContinuingAdapter(ISelect<TIn, ValueTask<TOut>> previous) => _previous = previous;

	public async ValueTask<Stop<TOut>> Get(Stop<TIn> parameter)
	{
		var (subject, token) = parameter;
		return new(await _previous.Off(subject), token);
	}
}