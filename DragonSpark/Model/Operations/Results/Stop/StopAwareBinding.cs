using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results.Stop;

sealed class StopAwareBinding<TIn, TOut> : ISelecting<CancellationToken, TOut>
{
	readonly ISelect<Stop<TIn>, ValueTask<TOut>>     _select;
	readonly Func<CancellationToken, ValueTask<TIn>> _input;
	readonly bool                                    _capture;

	public StopAwareBinding(ISelect<Stop<TIn>, ValueTask<TOut>> select, TIn instance) : this(select, () => instance) {}

	public StopAwareBinding(ISelect<Stop<TIn>, ValueTask<TOut>> select, Func<TIn> input)
		: this(select, _ => input().ToOperation()) {}

	public StopAwareBinding(ISelect<Stop<TIn>, ValueTask<TOut>> select, Func<CancellationToken, ValueTask<TIn>> input,
	                        bool capture = false)
	{
		_select  = select;
		_input   = input;
		_capture = capture;
	}

	public async ValueTask<TOut> Get(CancellationToken parameter)
	{
		var input  = await _input(parameter).ConfigureAwait(_capture);
		var result = await _select.Off(new(input, parameter));
		return result;
	}
}