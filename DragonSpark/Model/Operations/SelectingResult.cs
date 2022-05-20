using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class SelectingResult<TIn, TOut> : IResulting<TOut>
{
	readonly IResulting<TIn>            _previous;
	readonly Func<TIn, ValueTask<TOut>> _select;

	public SelectingResult(IResulting<TIn> previous, Func<TIn, ValueTask<TOut>> select)
	{
		_previous = previous;
		_select   = @select;
	}

	public async ValueTask<TOut> Get()
	{
		var previous = await _previous.Await();
		var result   = await _select(previous).ConfigureAwait(false);
		return result;
	}
}