using System;

namespace DragonSpark.Model.Selection;

public class DelegatedAssume<TIn, TOut> : ISelect<TIn, TOut>
{
	readonly Func<Func<TIn, TOut>> _source;

	public DelegatedAssume(Func<Func<TIn, TOut>> source) => _source = source;

	public TOut Get(TIn parameter) => _source()(parameter);
}