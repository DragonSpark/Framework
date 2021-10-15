using System;

namespace DragonSpark.Model.Selection;

sealed class Introduce<TIn, TOut> : ISelect<TIn, (TIn, TOut)>
{
	readonly Func<TIn, TOut> _select;

	public Introduce(ISelect<TIn, TOut> select) : this(select.Get) {}

	public Introduce(Func<TIn, TOut> select) => _select = select;

	public (TIn, TOut) Get(TIn parameter) => (parameter, _select(parameter));
}