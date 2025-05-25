using DragonSpark.Compose;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results.Stop;

public class SelectingResult<TIn, TOut> : IStopAware<TOut>
{
	readonly Await<CancellationToken, TIn>    _in;
	readonly Func<Stop<TIn>, ValueTask<TOut>> _select;

	protected SelectingResult(IStopAware<TIn> @in, Func<Stop<TIn>, ValueTask<TOut>> select) : this(@in.Off, select) {}

	protected SelectingResult(Await<CancellationToken, TIn> @in, Func<Stop<TIn>, ValueTask<TOut>> select)
	{
		_in     = @in;
		_select = @select;
	}

	public async ValueTask<TOut> Get(CancellationToken parameter)
	{
		var previous = await _in(parameter);
		var result   = await _select(new(previous, parameter)).Off();
		return result;
	}
}