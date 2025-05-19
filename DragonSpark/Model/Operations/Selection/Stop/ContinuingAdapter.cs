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

// TODO
sealed class StopAdapter<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly ISelect<TIn, ValueTask<TOut>> _previous;

	public StopAdapter(ISelect<TIn, ValueTask<TOut>> previous) => _previous = previous;

	public ValueTask<TOut> Get(Stop<TIn> parameter)
	{
		var (subject, _) = parameter;
		return _previous.Get(subject);
	}
}