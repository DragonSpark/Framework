using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

sealed class Binding<TIn, TOut> : IResulting<TOut>
{
	readonly ISelecting<TIn, TOut> _select;
	readonly Func<ValueTask<TIn>>  _parameter;
	readonly bool                  _capture;

	public Binding(ISelecting<TIn, TOut> select, Func<ValueTask<TIn>> parameter, bool capture = false)
	{
		_select    = select;
		_parameter = parameter;
		_capture   = capture;
	}

	public async ValueTask<TOut> Get()
	{
		var parameter = await _parameter().ConfigureAwait(_capture);
		var result    = await _select.Off(parameter);
		return result;
	}
}