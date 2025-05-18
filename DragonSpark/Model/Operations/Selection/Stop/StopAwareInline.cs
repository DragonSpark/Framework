using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class StopAwareInline<TIn, TFrom, TTo> : IStopAware<TIn, TTo>
{
	readonly Func<TIn, TFrom>                  _from;
	readonly Func<Stop<TFrom>, ValueTask<TTo>> _to;

	public StopAwareInline(ISelect<TIn, TFrom> from, ISelect<Stop<TFrom>, ValueTask<TTo>> to)
		: this(from.Get, to.Get) {}

	public StopAwareInline(Func<TIn, TFrom> from, Func<Stop<TFrom>, ValueTask<TTo>> to)
	{
		_from = @from;
		_to   = to;
	}

	public async ValueTask<TTo> Get(Stop<TIn> parameter)
	{
		var from   = _from(parameter);
		var to     = _to(new(from, parameter));
		var result = to.IsCompletedSuccessfully ? to.Result : await to.Off();
		return result;
	}
}