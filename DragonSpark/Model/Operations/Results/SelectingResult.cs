using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public class SelectingResult<TIn, TOut> : IResulting<TOut>
{
	readonly AwaitOf<TIn>               _in;
	readonly Func<TIn, ValueTask<TOut>> _select;

	public SelectingResult(IResulting<TIn> @in, Func<TIn, ValueTask<TOut>> select) : this(@in.Await, select) {}

	public SelectingResult(AwaitOf<TIn> @in, Func<TIn, ValueTask<TOut>> select)
	{
		_in     = @in;
		_select = @select;
	}

	public async ValueTask<TOut> Get()
	{
		var previous = await _in();
		var result   = await _select(previous).ConfigureAwait(false);
		return result;
	}
}