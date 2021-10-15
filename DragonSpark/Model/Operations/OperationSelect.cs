using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

sealed class OperationSelect<TIn, TOut> : ISelect<ValueTask<TIn>, ValueTask<TOut>>
{
	readonly Func<TIn, TOut> _select;
	readonly bool            _capture;

	public OperationSelect(Func<TIn, TOut> select, bool capture = false)
	{
		_select  = select;
		_capture = capture;
	}

	public async ValueTask<TOut> Get(ValueTask<TIn> parameter)
	{
		if (parameter.IsCompletedSuccessfully)
		{
			return _select(parameter.Result);
		}

		var input  = await parameter.ConfigureAwait(_capture);
		var result = _select(input);
		return result;
	}
}

sealed class OperationSelect<T> : ISelect<ValueTask<T>, ValueTask>
{
	readonly Func<T, ValueTask> _select;
	readonly bool               _capture;

	public OperationSelect(Func<T, ValueTask> select, bool capture = false)
	{
		_select  = select;
		_capture = capture;
	}

	public async ValueTask Get(ValueTask<T> parameter)
	{
		var input = parameter.IsCompletedSuccessfully ? parameter.Result : await parameter.ConfigureAwait(_capture);
		await _select(input).ConfigureAwait(_capture);
	}
}