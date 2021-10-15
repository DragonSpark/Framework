using System;

namespace DragonSpark.Model.Selection;

sealed class DelegateSelector<TIn, TOut> : ISelect<ISelect<TIn, TOut>, Func<TIn, TOut>>
{
	public static DelegateSelector<TIn, TOut> Default { get; } = new DelegateSelector<TIn, TOut>();

	DelegateSelector() {}

	public Func<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => parameter.Get;
}