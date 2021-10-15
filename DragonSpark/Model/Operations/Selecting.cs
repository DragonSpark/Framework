using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Selecting<TIn, TOut> : Select<TIn, ValueTask<TOut>>, ISelecting<TIn, TOut>
{
	public Selecting(ISelect<TIn, ValueTask<TOut>> select) : this(select.Get) {}

	public Selecting(Func<TIn, ValueTask<TOut>> select) : base(select) {}
}

public class Selecting<TIn, TFrom, TTo> : ISelecting<TIn, TTo>
{
	readonly Func<TIn, ValueTask<TFrom>> _from;
	readonly Func<TFrom, ValueTask<TTo>> _to;

	public Selecting(ISelect<TIn, ValueTask<TFrom>> from, ISelect<TFrom, ValueTask<TTo>> to)
		: this(@from.Get, to.Get) {}

	public Selecting(Func<TIn, ValueTask<TFrom>> from, Func<TFrom, ValueTask<TTo>> to)
	{
		_from = @from;
		_to   = to;
	}

	public async ValueTask<TTo> Get(TIn parameter)
	{
		var from   = _from(parameter);
		var first  = from.IsCompletedSuccessfully ? from.Result : await from.ConfigureAwait(false);
		var to     = _to(first);
		var result = to.IsCompletedSuccessfully ? to.Result : await to.ConfigureAwait(false);
		return result;
	}
}