using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class StopAware<TIn, TOut> : Selecting<Stop<TIn>, TOut>, IStopAware<TIn, TOut>
{
	public StopAware(ISelect<Stop<TIn>, ValueTask<TOut>> select) : base(select) {}

	public StopAware(Func<Stop<TIn>, ValueTask<TOut>> select) : base(select) {}
}

public class StopAware<TIn, TFrom, TTo> : IStopAware<TIn, TTo>
{
	readonly Func<Stop<TIn>, ValueTask<TFrom>> _from;
	readonly Func<Stop<TFrom>, ValueTask<TTo>> _to;

	public StopAware(ISelect<Stop<TIn>, ValueTask<TFrom>> from, ISelect<Stop<TFrom>, ValueTask<TTo>> to)
		: this(from.Get, to.Get) {}

	public StopAware(Func<Stop<TIn>, ValueTask<TFrom>> from, Func<Stop<TFrom>, ValueTask<TTo>> to)
	{
		_from = @from;
		_to   = to;
	}

	public async ValueTask<TTo> Get(Stop<TIn> parameter)
	{
		var from   = _from(parameter);
		var first  = from.IsCompletedSuccessfully ? from.Result : await from.Off();
		var to     = _to(new(first, parameter.Token));
		var result = to.IsCompletedSuccessfully ? to.Result : await to.Off();
		return result;
	}
}